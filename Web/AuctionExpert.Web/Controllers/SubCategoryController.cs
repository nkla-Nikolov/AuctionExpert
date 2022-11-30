namespace AuctionExpert.Web.Controllers
{
    using System.Collections.Generic;
    using AuctionExpert.Services.Data.SubCategory;
    using AuctionExpert.Web.ViewModels.SubCategory;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
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
