namespace AuctionExpert.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using AuctionExpert.Data;
    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Data.Repositories;
    using AuctionExpert.Services.Data.Auction;
    using AuctionExpert.Services.Data.Bid;
    using AuctionExpert.Services.Data.Image;
    using AuctionExpert.Services.Data.Review;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Auction;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class AuctionServiceTests
    {
        private IDeletableEntityRepository<Auction> auctionRepository;
        private IDeletableEntityRepository<Image> imageRepository;
        private IAuctionService auctionService;
        private IImageService imageService;
        private IBidService bidService;
        private IAuctionReviewService reviewService;
        private ApplicationDbContext context;

        public AuctionServiceTests()
        {
            this.SetupMapper();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AuctionsDb")
                .Options;

            this.context = new ApplicationDbContext(options);
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectNumber()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Equal(1, await this.auctionService.GetAllAuctions<MyAuctionsViewModel>().CountAsync());
        }

        [Fact]
        public async Task GetAuctionShouldReturnNull()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Null(await this.auctionService.GetAuctionById(2));
            Assert.Null(await this.auctionService.GetAuctionById(3));
        }

        [Fact]
        public async Task GetAuctionShouldReturnCorrectEntity()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.NotNull(await this.auctionService.GetAuctionById(1));
        }

        [Fact]
        public async Task GetAuctionsByOwnerShouldReturnOnlyOwnersAuctions()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 2,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 3,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Equal(3, await this.auctionService.GetAuctionsByOwnerId<MyAuctionsViewModel>(1, "owner", 3).CountAsync());
            Assert.Equal(2, await this.auctionService.GetAuctionsByOwnerId<MyAuctionsViewModel>(1, "owner", 2).CountAsync());
        }

        [Fact]
        public async Task GetAuctionsByOwnerShouldBeEmptyIfOwnerDoesNotExist()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 2,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 3,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Empty(this.auctionService.GetAuctionsByOwnerId<MyAuctionsViewModel>(1, "some other owner", 3));
            Assert.Empty(this.auctionService.GetAuctionsByOwnerId<MyAuctionsViewModel>(1, "other owner", 2));
        }

        [Fact]
        public async Task GetAuctionsByCategoryIdShouldReturnCorrectNumber()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CategoryId = 1,
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 2,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CategoryId = 1,
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 3,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CategoryId = 1,
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Equal(3, await this.auctionService.GetAllAuctionsByCategoryId<MyAuctionsViewModel>(1, 3, 1).CountAsync());
            Assert.Equal(2, await this.auctionService.GetAllAuctionsByCategoryId<MyAuctionsViewModel>(1, 2, 1).CountAsync());
        }

        [Fact]
        public async Task GetAuctionsByCategoryIdShouldBeEmpty()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CategoryId = 1,
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 2,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CategoryId = 1,
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 3,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CategoryId = 1,
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Empty(this.auctionService.GetAllAuctionsByCategoryId<MyAuctionsViewModel>(1, 3, 2));
        }

        [Fact]
        public async Task GetAuctionsByCountryIdShouldReturnCorrectNumber()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CountryId = 1,
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 2,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CountryId = 1,
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 3,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CountryId = 1,
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Equal(3, await this.auctionService.GetAllAuctionsByCountryId<MyAuctionsViewModel>(1).CountAsync());
            Assert.NotEqual(2, await this.auctionService.GetAllAuctionsByCountryId<MyAuctionsViewModel>(1).CountAsync());
        }

        [Fact]
        public async Task GetAuctionsByCountryIdShouldReturnEmpty()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CountryId = 1,
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 2,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CountryId = 1,
            });
            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 3,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
                CountryId = 2,
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Equal(0, await this.auctionService.GetAllAuctionsByCountryId<MyAuctionsViewModel>(3).CountAsync());
            Assert.Equal(1, await this.auctionService.GetAllAuctionsByCountryId<MyAuctionsViewModel>(2).CountAsync());
            Assert.Empty(this.auctionService.GetAllAuctionsByCountryId<MyAuctionsViewModel>(4));
        }

        [Fact]
        public async Task DeleteShouldThrowExceptionIfAuctionIsNull()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.auctionService.DeleteAsync(2));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.auctionService.DeleteAsync(-1));
        }

        [Fact]
        public async Task DeleteShouldWorkCorrectly()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.SaveChangesAsync();

            await this.auctionService.DeleteAsync(1);

            Assert.Equal(0, await this.auctionService.GetAllAuctions<MyAuctionsViewModel>().CountAsync());
        }

        [Fact]
        public async Task PlaceBidShouldReturnExcetionIfAuctionIsNull()
        {
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.auctionService = new AuctionService(this.auctionRepository, this.imageService, this.bidService, this.reviewService);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 2,
                Title = "Auction",
                Description = "Description",
                OwnerId = "owner",
            });
            await this.auctionRepository.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.auctionService.PlaceBidAsync(10, "owner", 0));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.auctionService.PlaceBidAsync(10, "owner", 1));
        }

        private void SetupMapper()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load("AuctionExpert.Web.ViewModels"));
        }
    }
}
