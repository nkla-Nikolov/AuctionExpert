namespace AuctionExpert.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using AuctionExpert.Data;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Data.Repositories;
    using AuctionExpert.Services.Data.Bid;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Bid;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class BidServiceTests
    {
        private EfDeletableEntityRepository<Bid> bidRepository;
        private EfDeletableEntityRepository<Auction> auctionRepository;
        private IBidService bidService;

        public BidServiceTests()
        {
            this.SetupMapper();
            this.SetupDatabase();

            this.bidService = new BidService(this.bidRepository);
        }

        [Fact]
        public async Task GetAllBidsShouldReturnCorrectNumber()
        {
            var auction = new Auction()
            {
                Id = 12,
                Title = "test",
                Description = "test",
                OwnerId = "test",
            };

            auction.Bids.Add(new Bid() { AuctionId = 12, BidderId = "SomeGuid" });
            auction.Bids.Add(new Bid() { AuctionId = 12, BidderId = "SomeGuid" });
            auction.Bids.Add(new Bid() { AuctionId = 12, BidderId = "SomeGuid" });
            await this.auctionRepository.AddAsync(auction);
            await this.auctionRepository.SaveChangesAsync();

            Assert.Equal(3, this.bidService.GetAllBidsByAuctionId<BidListModel>(12).Count());
        }

        [Fact]
        public void GetAllBidsShouldBeEmptyIfAuctionIdIsNotFound()
        {
            Assert.Empty(this.bidService.GetAllBidsByAuctionId<BidListModel>(-1));
            Assert.Empty(this.bidService.GetAllBidsByAuctionId<BidListModel>(0));
            Assert.Empty(this.bidService.GetAllBidsByAuctionId<BidListModel>(1));
        }

        [Fact]
        public async Task GetAllBidsShouldBeOrderedByDescending()
        {
            var auction = new Auction()
            {
                Id = 5,
                Title = "test",
                Bids = new List<Bid>(),
                Description = "test",
                OwnerId = "test",
            };

            auction.Bids.Add(new Bid() { AuctionId = 5, BidderId = "SomeGuid", MoneyPlaced = 10 });
            auction.Bids.Add(new Bid() { AuctionId = 5, BidderId = "SomeGuid", MoneyPlaced = 100 });
            auction.Bids.Add(new Bid() { AuctionId = 5, BidderId = "SomeGuid", MoneyPlaced = 78 });

            await this.auctionRepository.AddAsync(auction);
            await this.auctionRepository.SaveChangesAsync();

            var result = this.bidService.GetAllBidsByAuctionId<BidListModel>(5).First().MoneyPlaced;

            Assert.Equal(100, result);
        }

        [Fact]
        public async Task GetLastHighestBidShouldReturnZeroIfNoElements()
        {
            var auction = new Auction()
            {
                Id = 3,
                Title = "test",
                Description = "test",
                OwnerId = "GuidedId",
            };

            Assert.Equal(0, await this.bidService.GetLastHighestBid(3));
            Assert.Equal(0, await this.bidService.GetLastHighestBid(4));
        }

        private void SetupMapper()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load("AuctionExpert.Web.ViewModels"));
        }

        private void SetupDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BidsTestDb").Options;
            var context = new ApplicationDbContext(options);
            this.bidRepository = new EfDeletableEntityRepository<Bid>(context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(context);
        }
    }
}
