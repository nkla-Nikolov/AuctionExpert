namespace AuctionExpert.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Image;
    using AuctionExpert.Web.ViewModels.Profile;
    using Microsoft.EntityFrameworkCore;

    using static AuctionExpert.Common.GlobalConstants.AuctionConstraintsAndMessages;

    public class AuctionService : IAuctionService
    {
        private readonly IDeletableEntityRepository<Auction> auctionRepository;
        private readonly IImageService imageService;
        private readonly IBidService bidService;

        public AuctionService(
            IDeletableEntityRepository<Auction> auctionRepository,
            IImageService imageService,
            IBidService bidService)
        {
            this.auctionRepository = auctionRepository;
            this.imageService = imageService;
            this.bidService = bidService;
        }

        public async Task CreateAsync(AddAuctionViewModel model, ApplicationUser user)
        {
            var auction = new Auction()
            {
                AuctionType = model.Type,
                CountryId = user.CountryId,
                Duration = model.Duration,
                ClosesIn = DateTime.UtcNow.AddDays(model.Duration),
                Description = model.Description,
                OwnerId = user.Id,
                StartPrice = model.StartPrice,
                SubCategoryId = model.SubCateogoryId,
                Title = model.Title,
            };

            var images = await this.imageService.UploadImages(model.Images);
            auction.Images = images.ToList();

            await this.auctionRepository.AddAsync(auction);
            await this.auctionRepository.SaveChangesAsync();
        }

        public IQueryable<HomeAuctionViewModel> GetAllAuctionsAsHomeModel()
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Select(x => new HomeAuctionViewModel()
                {
                    Id = x.Id,
                    AuthorName = x.Owner.FirstName,
                    ClosesIn = x.ClosesIn,
                    LastBid = x.Bids.Count == 0 ? x.StartPrice : x.Bids.OrderByDescending(x => x.MoneyPlaced).First().MoneyPlaced,
                    MainImage = x.Images.First().UrlPath,
                    Title = x.Title,
                    Views = x.Views,
                });
        }

        public async Task<Auction> GetAuctionById(int auctionId)
        {
            return await this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.Id == auctionId)
                .FirstOrDefaultAsync();
        }

        public async Task<DetailViewModel> GetDetailAuctionModelByIdAsync(int auctionId)
        {
            var auction = await this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.Id == auctionId)
                .Select(x => new DetailViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    BiddingPrice = x.Bids.Count == 0 ? x.StartPrice : x.Bids.OrderByDescending(x => x.MoneyPlaced).First().MoneyPlaced,
                    Bidders = x.Bids
                    .ToList()
                    .Select(b => new BidderViewModel()
                    {
                        Username = b.Bidder.UserName,
                        MoneyPlaced = b.MoneyPlaced,
                        TimePlaced = DateTime.UtcNow - b.TimePlaced,
                    }),
                    Images = x.Images
                    .ToList()
                    .Select(x => new DetailsImageViewModel()
                    {
                        ImageUrl = x.UrlPath,
                    }),
                })
                .FirstOrDefaultAsync();

            if (auction == null)
            {
                throw new ArgumentNullException(AuctionDoesNotExist);
            }

            return auction;
        }

        public async Task PlaceBidAsync(int? currentBid, string userId, int auctionId)
        {
            var auction = await this.GetAuctionById(auctionId);
            var bids = await this.bidService.GetAllBidsByAuctionIdAsync(auctionId);

            if (auction == null)
            {
                throw new ArgumentNullException(AuctionDoesNotExist);
            }

            if (currentBid == null)
            {
                throw new ArgumentNullException(InvalidInputValueForBid);
            }

            if (bids.Any() && currentBid <= bids[0].MoneyPlaced)
            {
                throw new InvalidOperationException(InputBidIsLessThanLastOne);
            }

            if (!bids.Any() && currentBid <= auction.StartPrice)
            {
                throw new InvalidOperationException(InputBidIsLessThanAuctionStartPrice);
            }

            auction.Bids.Add(new Bid()
            {
                BidderId = userId,
                MoneyPlaced = (decimal)currentBid,
                TimePlaced = DateTime.UtcNow,
            });

            this.auctionRepository.Update(auction);
            await this.auctionRepository.SaveChangesAsync();
        }

        public IQueryable<MyAuctionsViewModel> GetAuctionsByOwnerId(string ownerId)
        {
            return this.auctionRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.OwnerId == ownerId)
                .Select(x => new MyAuctionsViewModel()
                {
                    HighestBid = x.Bids.Count == 0 ? 0 : x.Bids.OrderByDescending(x => x.MoneyPlaced).First().MoneyPlaced,
                    ImageUrl = x.Images.First().UrlPath,
                    StartPrice = x.StartPrice,
                    Type = x.AuctionType.ToString().StartsWith("Fixed") ? "Fixed Price" : "Standard Auction",
                    Status = x.IsDeleted == true ? "Finished" : "Active",
                });
        }
    }
}
