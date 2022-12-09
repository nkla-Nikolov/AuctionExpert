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

        public IQueryable<T> GetAllAuctionsByCountryId<T>(int countryId)
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.CountryId == countryId)
                .To<T>();
        }

        public async Task DeleteAsync(int auctionId)
        {
            var auction = await this.GetAuctionById<Auction>(auctionId);

            if (auction == null)
            {
                throw new ArgumentNullException();
            }

            this.auctionRepository.Delete(auction);
            await this.auctionRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(AddAuctionViewModel model, ApplicationUser user)
        {
            var auction = new Auction()
            {
                AuctionType = model.AuctionType,
                CountryId = user.CountryId,
                Duration = model.Duration,
                Condition = model.Condition,
                StepAmount = model.StepAmount,
                ClosesIn = DateTime.UtcNow.AddDays(model.Duration),
                Description = model.Description,
                OwnerId = user.Id,
                StartPrice = model.StartPrice,
                CategoryId = model.CategoryId,
                SubCategoryId = model.SubCateogoryId,
                Title = model.Title,
            };

            var images = await this.imageService.UploadImages(model.Images);
            auction.Images = images.ToList();

            await this.auctionRepository.AddAsync(auction);
            await this.auctionRepository.SaveChangesAsync();
        }

        public async Task<DetailViewModel> GetDetailAuctionModelByIdAsync(int auctionId)
        {
            var highestBid = await this.bidService
                .GetLastHighestBid(auctionId);

            var comments = await this.reviewService
                .GetAllReviewsOnAuction<ReviewViewModel>(auctionId)
                .OrderByDescending(x => x.DatePlaced)
                .ToListAsync();

            var bidders = await this.bidService
                .GetAllBidsByAuctionId<BidderViewModel>(auctionId)
                .ToListAsync();

            var images = await this.imageService
                .GetAllImages<DetailsImageViewModel>(auctionId)
                .ToListAsync();

            return await this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.Id == auctionId)
                .Select(x => new DetailViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    CategoryName = x.Category.Name,
                    SubCategoryName = x.SubCategory.Name,
                    Condition = x.Condition.ToString(),
                    StepAmount = x.StepAmount,
                    Description = x.Description,
                    BiddingPrice = x.Bids.Count == 0 ? x.StartPrice : highestBid,
                    Comments = comments,
                    Bidders = bidders,
                    Images = images,
                    AuctionType = x.AuctionType,
                })
                .FirstOrDefaultAsync();
        }

        public async Task PlaceBidAsync(int? currentBid, string userId, Auction auction)
        {
            var lastBid = await this.bidService.GetLastHighestBid(auction.Id);

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

            this.auctionRepository.Update(auction);
            await this.auctionRepository.SaveChangesAsync();
        }

        public async Task EditAuction(int auctionId, EditAuctionInputModel model)
        {
            var auction = await this.auctionRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction == null)
            {
                throw new NullReferenceException();
            }

            var images = await this.imageService.UploadImages(model.Images);

            auction.AuctionType = model.AuctionType;
            auction.Condition = model.Condition;
            auction.Description = model.Description;
            auction.StartPrice = model.StartPrice;
            auction.StepAmount = model.StepAmount;
            auction.CategoryId = model.CategoryId;
            auction.SubCategoryId = model.SubCateogoryId;
            auction.Title = model.Title;
            auction.Images = images.ToList();

            this.auctionRepository.Update(auction);
            await this.auctionRepository.SaveChangesAsync();
        }
    }
}
