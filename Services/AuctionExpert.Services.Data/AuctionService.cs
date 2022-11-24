namespace AuctionExpert.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Bid;
    using AuctionExpert.Web.ViewModels.Image;
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

        public IQueryable<T> GetAllAuctions<T>()
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .To<T>();
        }

        public async Task<T> GetAuctionById<T>(int auctionId)
        {
            return await this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.Id == auctionId)
                .To<T>()
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
                        UrlPath = x.UrlPath,
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
            var auction = await this.GetAuctionById<Auction>(auctionId);
            var bids = await this.bidService.GetAllBidsByAuctionId<BidListModel>(auctionId).ToListAsync();

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

        public IQueryable<T> GetAuctionsByOwnerId<T>(string ownerId)
        {
            return this.auctionRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.OwnerId == ownerId)
                .To<T>();
        }

        public IQueryable<T> GetAllAuctionsByCategoryId<T>(int categoryId)
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.SubCategory.CategoryId == categoryId)
                .To<T>();
        }

        public async Task DeteleAsync(int auctionId)
        {
            var auction = await this.GetAuctionById<Auction>(auctionId);

            if (auction == null)
            {
                throw new NullReferenceException(AuctionDoesNotExist);
            }

            this.auctionRepository.Delete(auction);
            await this.auctionRepository.SaveChangesAsync();
        }
    }
}
