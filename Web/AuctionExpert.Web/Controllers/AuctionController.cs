namespace AuctionExpert.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Auction;
    using AuctionExpert.Services.Data.Category;
    using AuctionExpert.Services.Data.Review;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class AuctionController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IAuctionService auctionService;
        private readonly IReviewService reviewService;
        private readonly UserManager<ApplicationUser> userManager;

        public AuctionController(
            ICategoryService categoryService,
            IAuctionService auctionService,
            IReviewService reviewService,
            UserManager<ApplicationUser> userManager)
        {
            this.categoryService = categoryService;
            this.auctionService = auctionService;
            this.reviewService = reviewService;
            this.userManager = userManager;
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
            var auction = await this.auctionService.GetDetailAuctionModelByIdAsync(auctionId);

            if (auction == null)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            return this.View(auction);
        }

        [HttpPost]
        public async Task<IActionResult> Details(DetailViewModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var auction = await this.auctionService.GetAuctionById<Auction>(model.Id);

            if (auction == null)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            try
            {
                await this.auctionService.PlaceBidAsync(model.CurrentBid, userId, auction);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                model = await this.auctionService.GetDetailAuctionModelByIdAsync(auction.Id);

                return this.View(model);
            }

            return this.RedirectToAction(nameof(this.Details), new { auctionId = auction.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Browse(int categoryId)
        {
            if (!await this.categoryService.Exist(categoryId))
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            var auctions = await this.auctionService
                .GetAllAuctionsByCategoryId<HomeAuctionViewModel>(categoryId)
                .ToListAsync();

            return this.View(auctions);
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

            return this.RedirectToAction("Dashboard", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Comment(int auctionId, DetailViewModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await this.reviewService.CommentOnAuction(auctionId, model.Comment, userId);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            return this.RedirectToAction(nameof(this.Details), new { auctionId = auctionId });
        }
    }
}
