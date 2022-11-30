namespace AuctionExpert.Services.Data.Category
{
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public CategoryService(IDeletableEntityRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public Task<bool> Exist(int categoryId)
        {
            return this.categoryRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.Id == categoryId);
        }

        public IQueryable<T> GetAllCategories<T>()
        {
            return this.categoryRepository
                .AllAsNoTracking()
                .To<T>();
        }
    }
}
