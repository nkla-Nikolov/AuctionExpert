namespace AuctionExpert.Web.Controllers
{
    using System.Collections.Generic;

    using AuctionExpert.Services.Data;
    using AuctionExpert.Web.ViewModels.SubCategory;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoriesService subCategoryService;

        public SubCategoryController(ISubCategoriesService subCategoryService)
        {
            this.subCategoryService = subCategoryService;
        }

        [HttpGet]
        public IEnumerable<SubCategoryListModel> Get(int id)
        {
            return this.subCategoryService.GetAllByCategoryId<SubCategoryListModel>(id);
        }
    }
}
