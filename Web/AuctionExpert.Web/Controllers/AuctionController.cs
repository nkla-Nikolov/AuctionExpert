namespace AuctionExpert.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AuctionExpert.Common;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using static AuctionExpert.Common.GlobalConstants.AuctionConstraintsAndMessages;

    public class AuctionController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IAuctionService auctionService;
        private readonly UserManager<ApplicationUser> userManager;

        public AuctionController(
            ICategoryService categoryService,
            IAuctionService auctionService,
            UserManager<ApplicationUser> userManager)
        {
            this.categoryService = categoryService;
            this.auctionService = auctionService;
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
                throw new NullReferenceException(AuctionDoesNotExist);
            }

            return this.View(auction);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBid(DetailViewModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await this.auctionService.PlaceBidAsync(model.CurrentBid, userId, model.Id);
            }
            catch (Exception)
            {
                this.TempData[GlobalConstants.MessageConstants.ErrorMessage] = "Something went wrong!";
            }

            return this.RedirectToAction(nameof(this.Details), new { auctionId = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Browse(int categoryId)
        {
            var auctions = await this.auctionService.GetAllAuctionsByCategoryId<HomeAuctionViewModel>(categoryId).ToListAsync();

            return this.View(auctions);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int auctionId)
        {
            try
            {
                await this.auctionService.DeteleAsync(auctionId);
            }
            catch (Exception)
            {
                throw;
            }

            return this.RedirectToAction("Dashboard", "User");
        }
    }
}
