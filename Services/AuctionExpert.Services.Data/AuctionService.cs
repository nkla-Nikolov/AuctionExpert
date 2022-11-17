namespace AuctionExpert.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Auction;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<List<HomeAuctionViewModel>> GetAllAuctionsAsHomeModel()
        {
            return await this.auctionRepository
                .AllAsNoTracking()
                .Select(x => new HomeAuctionViewModel()
                {
                    Id = x.Id,
                    AuthorName = x.Owner.FirstName,
                    ClosesIn = x.ClosesIn,
                    LastBid = x.Bids == null ? x.StartPrice : x.Bids.OrderByDescending(x => x.MoneyPlaced).First().MoneyPlaced,
                    MainImage = x.Images.First().UrlPath,
                    Title = x.Title,
                    Views = x.Views,
                })
                .ToListAsync();
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
                })
                .FirstOrDefaultAsync();

            if (auction == null)
            {
                throw new ArgumentNullException("The auction you are looking for does not exist");
            }

            return auction;
        }

        public async Task PlaceBidAsync(int currentBid, string userId, int auctionId)
        {
            var auction = await this.GetAuctionById(auctionId);
            var bids = await this.bidService.GetAllBidsByAuctionIdAsync(auctionId);

            if (auction == null)
            {
                throw new ArgumentNullException("The auction does not exist!");
            }

            if (bids.Any() && currentBid < bids[0].MoneyPlaced)
            {
                throw new InvalidOperationException("You cannot place bid that is lower than the last one!");
            }

            if (!bids.Any() && currentBid < auction.StartPrice)
            {
                throw new InvalidOperationException("Your bid should be higher than the auction's start price!");
            }

            auction.Bids.Add(new Bid()
            {
                BidderId = userId,
                MoneyPlaced = currentBid,
                TimePlaced = DateTime.UtcNow,
            });

            this.auctionRepository.Update(auction);
            await this.auctionRepository.SaveChangesAsync();
        }
    }
}
