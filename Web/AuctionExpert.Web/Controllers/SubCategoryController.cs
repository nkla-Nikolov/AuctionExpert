namespace AuctionExpert.Web.Controllers
{
    using System.Threading.Tasks;

    using AuctionExpert.Services.Data.SubCategory;
    using AuctionExpert.Web.ViewModels.SubCategory;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class SubCategoryController : BaseController
    {
        private readonly ISubCategoryService subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            this.subCategoryService = subCategoryService;
        }

        public async Task<IActionResult> GetSubCategoriesByCategoryId(int categoryId)
        {
            var subCategories = await this.subCategoryService
                .GetAllByCategoryId<SubCategoryListModel>(categoryId)
                .ToListAsync();

            if (subCategories.Count == 0)
            {
                return this.Json(new { Success = false, ErrorMessage = "There are no sub categories" });
            }

            return this.Json(new { Success = true, SubCategories = subCategories });
        }
    }
}
