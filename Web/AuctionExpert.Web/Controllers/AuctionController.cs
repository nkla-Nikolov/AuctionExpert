namespace AuctionExpert.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Factories.Auction;
    using AuctionExpert.Services.Data.Auction;
    using AuctionExpert.Services.Data.Category;
    using AuctionExpert.Services.Data.Review;
    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using NToastNotify;

    public class AuctionController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IAuctionService auctionService;
        private readonly IAuctionReviewService reviewService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuctionModelFactory auctionModelFactory;
        private readonly IToastNotification notificationService;
        private readonly IUserService userService;

        public AuctionController(
            ICategoryService categoryService,
            IAuctionService auctionService,
            IAuctionReviewService reviewService,
            UserManager<ApplicationUser> userManager,
            IAuctionModelFactory auctionModelFactory,
            IToastNotification notificationService,
            IUserService userService)
        {
            this.categoryService = categoryService;
            this.auctionService = auctionService;
            this.reviewService = reviewService;
            this.userManager = userManager;
            this.auctionModelFactory = auctionModelFactory;
            this.notificationService = notificationService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Sell()
        {
            var model = new AddAuctionViewModel();
            model.Categories = this.categoryService.GetAllCategories<CategoryListModel>();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sell(AddAuctionViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this.categoryService.GetAllCategories<CategoryListModel>();
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.auctionService.CreateAsync(model, user);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int auctionId)
        {
            try
            {
                var model = await this.auctionModelFactory.PrepareAuctionDetailViewModelAsync(auctionId);
                return this.View(model);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Details(DetailViewModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await this.auctionService.PlaceBidAsync(model.CurrentBid, userId, model.Id);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                model = await this.auctionModelFactory.PrepareAuctionDetailViewModelAsync(model.Id);

                return this.View(model);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            this.notificationService.AddSuccessToastMessage("Successfully placed a bid");
            return this.RedirectToAction(nameof(this.Details), new { auctionId = model.Id });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Browse(int categoryId, int id = 1)
        {
            if (!await this.categoryService.Exist(categoryId))
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            int itemsPerPage = 2;

            var auctions = await this.auctionService
                .GetAllAuctionsByCategoryId<HomeAuctionViewModel>(id, itemsPerPage, categoryId)
                .ToListAsync();

            var model = new PaginatedHomeAuctionViewModel()
            {
                Auctions = auctions,
                ItemsPerPage = itemsPerPage,
                PageNumber = id,
                CategoryId = categoryId,
                ItemsCount = this.auctionService.AllAuctionsInCategoryCount(categoryId),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int auctionId)
        {
            try
            {
                await this.auctionService.DeleteAsync(auctionId);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            this.notificationService.AddSuccessToastMessage("Successfully deleted auction");
            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Comment(int auctionId, DetailViewModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var auction = await this.auctionService.GetAuctionByIdAsync(auctionId);

            try
            {
                await this.reviewService.CommentOnAuction(auction, model.Comment, userId);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            this.notificationService.AddSuccessToastMessage("Successfully added new comment");
            return this.RedirectToAction(nameof(this.Details), new { auctionId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int auctionId)
        {
            var auction = await this.auctionService.GetAuctionByIdAsync(auctionId);
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (auction == null)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            if (currentUserId != auction.OwnerId)
            {
                return this.Unauthorized();
            }

            var model = await this.auctionModelFactory.PrepareEditAuctionInputModel(auction);
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int auctionId, EditAuctionInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, string.Empty);
                var auction = await this.auctionService.GetAuctionByIdAsync(auctionId);

                model = await this.auctionModelFactory.PrepareEditAuctionInputModel(auction);
                return this.View(model);
            }

            try
            {
                await this.auctionService.EditAuction(auctionId, model);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            this.notificationService.AddSuccessToastMessage("Successfully updated auction");
            return this.RedirectToAction(nameof(this.Details), new { auctionId });
        }

        [HttpPost]
        public async Task<IActionResult> LikeAuction(int auctionId, string userId)
        {
            var auction = await this.auctionService.GetAuctionByIdAsync(auctionId);
            var user = await this.userService.GetUserByIdAsync(userId);

            if (auction == null || user == null)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            await this.auctionService.LikeAuction(auction, user);
            return this.Json(new { Result = true });
        }
    }
}
