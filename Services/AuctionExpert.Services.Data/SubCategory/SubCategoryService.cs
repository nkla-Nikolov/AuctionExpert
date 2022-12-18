namespace AuctionExpert.Services.Data.SubCategory
{
    using System.Linq;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class SubCategoryService : ISubCategoryService
    {
        private readonly IDeletableEntityRepository<SubCategory> subCategoryRepository;

        public SubCategoryService(IDeletableEntityRepository<SubCategory> subCategoryRepository)
        {
            this.subCategoryRepository = subCategoryRepository;
        }

        public IQueryable<T> GetAllSubCategories<T>()
        {
            return subCategoryRepository.AllAsNoTracking().To<T>();
        }

        public IQueryable<T> GetAllByCategoryId<T>(int categoryId)
        {
            return subCategoryRepository
                .AllAsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .To<T>();
        }
    }
}
