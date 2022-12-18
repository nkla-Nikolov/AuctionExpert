namespace AuctionExpert.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using AuctionExpert.Data;
    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Data.Repositories;
    using AuctionExpert.Services.Data.Category;
    using AuctionExpert.Services.Data.Country;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Administration.Country;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CountryServiceTests
    {
        private IDeletableEntityRepository<Country> countryRepository;
        private ApplicationDbContext context;
        private ICountryService countryService;

        public CountryServiceTests()
        {
            this.SetupMapper();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CountryDb")
                .Options;

            this.context = new ApplicationDbContext(options);
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectNumber()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            await this.countryRepository.AddAsync(new Country()
            {
                Name = "Bulgaria",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Name = "Germany",
            });
            await this.countryRepository.SaveChangesAsync();

            Assert.Equal(2, await this.countryService.GetAllCountries<CountryListModel>().CountAsync());
            Assert.NotEqual(3, await this.countryService.GetAllCountries<CountryListModel>().CountAsync());
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectEntities()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            await this.countryRepository.AddAsync(new Country()
            {
                Name = "Bulgaria",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Name = "Germany",
            });
            await this.countryRepository.SaveChangesAsync();

            Assert.Equal("Bulgaria", this.countryService.GetAllCountries<CountryListModel>().First().Name);
            Assert.Equal("Germany", this.countryService.GetAllCountries<CountryListModel>().Last().Name);
        }

        [Fact]
        public async Task GetCountryByIdShouldReturnNull()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            await this.countryRepository.AddAsync(new Country()
            {
                Id = 1,
                Name = "Bulgaria",
            });
            await this.countryRepository.SaveChangesAsync();

            Assert.Null(await this.countryService.GetCountryById<CountryListModel>(2));
            Assert.Null(await this.countryService.GetCountryById<CountryListModel>(3));
            Assert.Null(await this.countryService.GetCountryById<CountryListModel>(4));
        }

        [Fact]
        public async Task GetCountryByIdShouldNotReturnNull()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            await this.countryRepository.AddAsync(new Country()
            {
                Id = 1,
                Name = "Bulgaria",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 2,
                Name = "Serbia",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 3,
                Name = "Greece",
            });
            await this.countryRepository.SaveChangesAsync();

            Assert.NotNull(await this.countryService.GetCountryById<CountryListModel>(1));
            Assert.NotNull(await this.countryService.GetCountryById<CountryListModel>(2));
            Assert.NotNull(await this.countryService.GetCountryById<CountryListModel>(3));
        }

        [Fact]
        public async Task PaginatedShouldReturnCorrectCount()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            await this.countryRepository.AddAsync(new Country()
            {
                Id = 1,
                Name = "Bulgaria",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 2,
                Name = "Serbia",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 3,
                Name = "Greece",
            });
            await this.countryRepository.SaveChangesAsync();

            Assert.Equal(1, await this.countryService.GetAllCountriesPaginated<CountryListModel>(1, 1).CountAsync());
            Assert.Equal(1, await this.countryService.GetAllCountriesPaginated<CountryListModel>(2, 1).CountAsync());
            Assert.Equal(1, await this.countryService.GetAllCountriesPaginated<CountryListModel>(3, 1).CountAsync());
        }

        [Fact]
        public async Task PaginatedShouldReturnWithOrderedIds()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            await this.countryRepository.AddAsync(new Country()
            {
                Id = 4,
                Name = "Bulgaria",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 1,
                Name = "Serbia",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 8,
                Name = "Greece",
            });
            await this.countryRepository.SaveChangesAsync();

            Assert.Equal(1, this.countryService.GetAllCountriesPaginated<CountryListModel>(1, 5).ToArray()[0].Id);
            Assert.Equal(4, this.countryService.GetAllCountriesPaginated<CountryListModel>(1, 5).ToArray()[1].Id);
            Assert.Equal(8, this.countryService.GetAllCountriesPaginated<CountryListModel>(1, 5).ToArray()[2].Id);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectNumber()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            await this.countryRepository.AddAsync(new Country()
            {
                Id = 1,
                Name = "Bulgaria",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 2,
                Name = "Serbia",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 3,
                Name = "Greece",
            });
            await this.countryRepository.SaveChangesAsync();

            Assert.Equal(3, this.countryService.GetCount());
        }

        [Fact]
        public async Task GetCountShouldReturnWrongNumber()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            await this.countryRepository.AddAsync(new Country()
            {
                Id = 1,
                Name = "Bulgaria",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 2,
                Name = "Serbia",
            });
            await this.countryRepository.AddAsync(new Country()
            {
                Id = 3,
                Name = "Greece",
            });
            await this.countryRepository.SaveChangesAsync();

            Assert.NotEqual(1, this.countryService.GetCount());
            Assert.NotEqual(2, this.countryService.GetCount());
            Assert.NotEqual(4, this.countryService.GetCount());
        }

        [Fact]
        public async Task DeleteCountryShouldDeleteCorrectly()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            var country = new Country()
            {
                Id = 5,
                Name = "Bulgaria",
            };
            await this.countryRepository.AddAsync(country);
            await this.countryRepository.SaveChangesAsync();

            Assert.Equal(1, this.countryService.GetCount());
            await this.countryService.DeleteCountry(5);
            await this.countryRepository.SaveChangesAsync();
            Assert.Equal(0, this.countryService.GetCount());
        }

        [Fact]
        public async Task DeleteCountryShouldReturnNull()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            var country = new Country()
            {
                Id = 5,
                Name = "Bulgaria",
            };
            await this.countryRepository.AddAsync(country);
            await this.countryRepository.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.countryService.DeleteCountry(4));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.countryService.DeleteCountry(6));
        }

        [Fact]
        public async Task AddCountryShouldWorkCorrectly()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            Assert.Equal(0, this.countryService.GetCount());

            await this.countryService.AddCountry("Bulgaria");
            await this.countryService.AddCountry("Romania");

            Assert.Equal(2, this.countryService.GetCount());
        }

        [Fact]
        public async Task AddCountryShouldThrowException()
        {
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.context);
            this.countryService = new CountryService(this.countryRepository);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.countryService.AddCountry(null));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.countryService.AddCountry(string.Empty));
        }

        private void SetupMapper()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load("AuctionExpert.Web.ViewModels"));
        }
    }
}
