namespace AuctionExpert.Services.Data.Auction
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Bid;
    using AuctionExpert.Services.Data.Image;
    using AuctionExpert.Services.Data.Review;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Image;
    using AuctionExpert.Web.ViewModels.Review;
    using Microsoft.EntityFrameworkCore;

    using static AuctionExpert.Common.GlobalConstants.AuctionConstraintsAndMessages;

    public class AuctionService : IAuctionService
    {
        private readonly IDeletableEntityRepository<Auction> auctionRepository;
        private readonly IImageService imageService;
        private readonly IBidService bidService;
        private readonly IReviewService reviewService;

        public AuctionService(
            IDeletableEntityRepository<Auction> auctionRepository,
            IImageService imageService,
            IBidService bidService,
            IReviewService reviewService)
        {
            this.auctionRepository = auctionRepository;
            this.imageService = imageService;
            this.bidService = bidService;
            this.reviewService = reviewService;
        }

        public IQueryable<T> GetAllAuctions<T>()
        {
            return auctionRepository
                .AllAsNoTracking()
                .To<T>();
        }

        public async Task<T> GetAuctionById<T>(int auctionId)
        {
            return await auctionRepository
                .AllAsNoTracking()
                .Where(x => x.Id == auctionId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAuctionsByOwnerId<T>(string ownerId)
        {
            return auctionRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.OwnerId == ownerId)
                .To<T>();
        }

        public IQueryable<T> GetAllAuctionsByCategoryId<T>(int categoryId)
        {
            return auctionRepository
                .AllAsNoTracking()
                .Where(x => x.SubCategory.CategoryId == categoryId)
                .To<T>();
        }

        public async Task DeleteAsync(int auctionId)
        {
            var auction = await GetAuctionById<Auction>(auctionId);

            if (auction == null)
            {
                throw new ArgumentNullException();
            }

            auctionRepository.Delete(auction);
            await auctionRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(AddAuctionViewModel model, ApplicationUser user)
        {
            var auction = new Auction()
            {
                AuctionType = model.Type,
                CountryId = user.CountryId,
                Duration = model.Duration,
                StepAmount = model.StepAmount,
                ClosesIn = DateTime.UtcNow.AddDays(model.Duration),
                Description = model.Description,
                OwnerId = user.Id,
                StartPrice = model.StartPrice,
                SubCategoryId = model.SubCateogoryId,
                Title = model.Title,
            };

            var images = await imageService.UploadImages(model.Images);
            auction.Images = images.ToList();

            await auctionRepository.AddAsync(auction);
            await auctionRepository.SaveChangesAsync();
        }

        public async Task<DetailViewModel> GetDetailAuctionModelByIdAsync(int auctionId)
        {
            var highestBid = await bidService
                .GetLastHighestBid(auctionId);

            var comments = await reviewService
                .GetAllCommentsOnAuction<CommentViewModel>(auctionId)
                .OrderByDescending(x => x.DatePlaced)
                .ToListAsync();

            var bidders = await bidService
                .GetAllBidsByAuctionId<BidderViewModel>(auctionId)
                .ToListAsync();

            var images = await imageService
                .GetAllImages<DetailsImageViewModel>(auctionId)
                .ToListAsync();

            return await auctionRepository
                .AllAsNoTracking()
                .Where(x => x.Id == auctionId)
                .Select(x => new DetailViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    StepAmount = x.StepAmount,
                    Description = x.Description,
                    BiddingPrice = x.Bids.Count == 0 ? x.StartPrice : highestBid,
                    Comments = comments,
                    Bidders = bidders,
                    Images = images,
                })
                .FirstOrDefaultAsync();
        }

        public async Task PlaceBidAsync(int? currentBid, string userId, Auction auction)
        {
            var lastBid = await bidService.GetLastHighestBid(auction.Id);

            if (currentBid == null)
            {
                throw new InvalidOperationException(InvalidInputBidValue);
            }

            if (currentBid < auction.StartPrice + auction.StepAmount ||
                currentBid < lastBid + auction.StepAmount)
            {
                throw new InvalidOperationException(LowerInputBid);
            }

            auction.Bids.Add(new Bid()
            {
                BidderId = userId,
                MoneyPlaced = (decimal)currentBid,
                TimePlaced = DateTime.UtcNow,
            });

            auctionRepository.Update(auction);
            await auctionRepository.SaveChangesAsync();
        }
    }
}
