namespace AuctionExpert.Services.Data.Tests
{
    using System.Reflection;
    using System.Threading.Tasks;
    using AuctionExpert.Data;
    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Data.Repositories;
    using AuctionExpert.Services.Data.Category;
    using AuctionExpert.Services.Data.City;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.City;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CityServiceTests
    {
        private IDeletableEntityRepository<City> cityRepository;
        private ApplicationDbContext context;
        private ICityService cityService;

        public CityServiceTests()
        {
            this.SetupMapper();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CityDb")
                .Options;

            this.context = new ApplicationDbContext(options);
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetCityByIdShouldReturnNullIfIdIsNull()
        {
            this.cityRepository = new EfDeletableEntityRepository<City>(this.context);
            this.cityService = new CityService(this.cityRepository);

            Assert.Null(await this.cityService.GetCityById<CityListModel>(1));
            Assert.Null(await this.cityService.GetCityById<CityListModel>(2));
            Assert.Null(await this.cityService.GetCityById<CityListModel>(3));
        }

        [Fact]
        public async Task GetCityShouldReturnCorrectEntity()
        {
            this.cityRepository = new EfDeletableEntityRepository<City>(this.context);
            this.cityService = new CityService(this.cityRepository);

            await this.cityRepository.AddAsync(new City()
            {
                Id = 1,
                Name = "Sofia",
            });
            await this.cityRepository.SaveChangesAsync();

            Assert.Equal("Sofia", this.cityService.GetCityById<CityListModel>(1).Result.Name);
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectNumber()
        {
            this.cityRepository = new EfDeletableEntityRepository<City>(this.context);
            this.cityService = new CityService(this.cityRepository);

            await this.cityRepository.AddAsync(new City()
            {
                Id = 1,
                Name = "Sofia",
                CountryId = 5,
            });
            await this.cityRepository.AddAsync(new City()
            {
                Id = 2,
                Name = "Varna",
                CountryId = 5,
            });
            await this.cityRepository.SaveChangesAsync();

            Assert.Equal(2, await this.cityService.GetAllCitiesByCountryId<CityListModel>(5).CountAsync());
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectCityNames()
        {
            this.cityRepository = new EfDeletableEntityRepository<City>(this.context);
            this.cityService = new CityService(this.cityRepository);

            await this.cityRepository.AddAsync(new City()
            {
                Id = 1,
                Name = "Sofia",
                CountryId = 5,
            });
            await this.cityRepository.AddAsync(new City()
            {
                Id = 2,
                Name = "Varna",
                CountryId = 5,
            });
            await this.cityRepository.SaveChangesAsync();

            Assert.Equal("Sofia", this.cityService.GetAllCitiesByCountryId<CityListModel>(5).FirstAsync().Result.Name);
            Assert.Equal("Varna", this.cityService.GetAllCitiesByCountryId<CityListModel>(5).LastAsync().Result.Name);
        }

        [Fact]
        public async Task GetAllShouldReturnNotCorrectNumber()
        {
            this.cityRepository = new EfDeletableEntityRepository<City>(this.context);
            this.cityService = new CityService(this.cityRepository);

            await this.cityRepository.AddAsync(new City()
            {
                Id = 1,
                Name = "Sofia",
                CountryId = 5,
            });
            await this.cityRepository.AddAsync(new City()
            {
                Id = 2,
                Name = "Varna",
                CountryId = 6,
            });
            await this.cityRepository.SaveChangesAsync();

            Assert.NotEqual(2, await this.cityService.GetAllCitiesByCountryId<CityListModel>(5).CountAsync());
        }

        private void SetupMapper()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load("AuctionExpert.Web.ViewModels"));
        }
    }
}
