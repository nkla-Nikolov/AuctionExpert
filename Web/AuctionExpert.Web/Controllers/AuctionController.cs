namespace AuctionExpert.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

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
    }
}
