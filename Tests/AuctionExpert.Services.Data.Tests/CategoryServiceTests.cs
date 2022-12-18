namespace AuctionExpert.Services.Data.Tests
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using AuctionExpert.Data;
    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Data.Repositories;
    using AuctionExpert.Services.Data.Category;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Category;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CategoryServiceTests
    {
        private IDeletableEntityRepository<Category> categoryRepository;
        private ApplicationDbContext context;
        private ICategoryService categoryService;

        public CategoryServiceTests()
        {
            this.SetupMapper();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CategoryDb")
                .Options;

            this.context = new ApplicationDbContext(options);
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectNumber()
        {
            this.categoryRepository = new EfDeletableEntityRepository<Category>(this.context);
            this.categoryService = new CategoryService(this.categoryRepository);

            await this.categoryRepository.AddAsync(new Category()
            {
                Id = 1,
                Name = "Test",
            });

            await this.categoryRepository.SaveChangesAsync();
            Assert.Equal(1, await this.categoryService.GetAllCategories<CategoryListModel>().CountAsync());
        }

        [Fact]
        public async Task ExistMethodShouldReturnTrueIfCategoryExist()
        {
            this.categoryRepository = new EfDeletableEntityRepository<Category>(this.context);
            this.categoryService = new CategoryService(this.categoryRepository);

            await this.categoryRepository.AddAsync(new Category()
            {
                Id = 1,
                Name = "Test",
            });

            await this.categoryRepository.SaveChangesAsync();
            Assert.True(await this.categoryService.Exist(1));
        }

        [Fact]
        public async Task ExistMethodShouldReturnFalseIfCategoryDoesNotExist()
        {
            this.categoryRepository = new EfDeletableEntityRepository<Category>(this.context);
            this.categoryService = new CategoryService(this.categoryRepository);

            await this.categoryRepository.AddAsync(new Category()
            {
                Id = 2,
                Name = "Test",
            });

            await this.categoryRepository.SaveChangesAsync();
            Assert.False(await this.categoryService.Exist(1));
        }

        [Fact]
        public async Task ExistMethodShouldReturnCorrectCategoryName()
        {
            this.categoryRepository = new EfDeletableEntityRepository<Category>(this.context);
            this.categoryService = new CategoryService(this.categoryRepository);

            await this.categoryRepository.AddAsync(new Category()
            {
                Id = 2,
                Name = "Testt",
            });

            await this.categoryRepository.SaveChangesAsync();
            Assert.Equal("Testt", this.categoryService.GetAllCategories<CategoryListModel>().First().Name);
        }

        private void SetupMapper()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load("AuctionExpert.Web.ViewModels"));
        }
    }
}
