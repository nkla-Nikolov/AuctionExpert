namespace AuctionExpert.Web.Controllers
{
    using AuctionExpert.Services.Data;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Mvc;

    public class AuctionController : BaseController
    {
        private readonly ICategoriesService categoryService;

        public AuctionController(
            ICategoriesService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Sell()
        {
            var model = new AddAuctionViewModel();
            model.Categories = this.categoryService.GetAllCategories<CategoryListModel>();

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Sell(AddAuctionViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Sell));
            }

            return this.RedirectToAction();
        }
    }
}
