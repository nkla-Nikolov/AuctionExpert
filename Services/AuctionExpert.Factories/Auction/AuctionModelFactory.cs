using AuctionExpert.Services.Data.Auction;
using AuctionExpert.Services.Data.Bid;
using AuctionExpert.Services.Data.Category;
using AuctionExpert.Services.Data.Image;
using AuctionExpert.Services.Data.Review;
using AuctionExpert.Web.ViewModels.Auction;
using AuctionExpert.Web.ViewModels.Category;
using AuctionExpert.Web.ViewModels.Image;
using AuctionExpert.Web.ViewModels.Review;
using Microsoft.EntityFrameworkCore;

namespace AuctionExpert.Factories.Auction
{
    public class AuctionModelFactory : IAuctionModelFactory
    {
        private readonly IBidService bidService;
        private readonly IAuctionService auctionService;
        private readonly IAuctionReviewService auctionReviewService;
        private readonly IImageService imageService;
        private readonly ICategoryService categoryService;

        public AuctionModelFactory(
            IBidService bidService,
            IAuctionService auctionService,
            IAuctionReviewService auctionReviewService,
            IImageService imageService,
            ICategoryService categoryService)
        {
            this.bidService = bidService;
            this.auctionService = auctionService;
            this.auctionReviewService = auctionReviewService;
            this.imageService = imageService;
            this.categoryService = categoryService;
        }

        public async Task<DetailViewModel> PrepareAuctionDetailViewModelAsync(int auctionId)
        {
            var auction = await this.auctionService
                .GetAuctionByIdAsync(auctionId);

            if (auction == null)
            {
                throw new ArgumentNullException();
            }

            var highestBid = await this.bidService
                .GetLastHighestBid(auctionId);

            var comments = await this.auctionReviewService
                .GetAllAuctionReviews<AuctionReviewViewModel>(auction)
                .OrderByDescending(x => x.DatePlaced)
                .ToListAsync();

            var bidders = await this.bidService
                .GetAllBidsByAuctionId<BidderViewModel>(auctionId)
                .ToListAsync();

            var images = await this.imageService
                .GetAllImages<DetailsImageViewModel>(auctionId)
                .ToListAsync();

            return new DetailViewModel
            {
                Id = auction.Id,
                Title = auction.Title,
                CategoryName = auction.Category.Name,
                SubCategoryName = auction.SubCategory.Name,
                Condition = auction.Condition.ToString(),
                StepAmount = auction.StepAmount,
                Description = auction.Description,
                BiddingPrice = auction.Bids.Count == 0 ? auction.StartPrice : highestBid,
                Comments = comments,
                Bidders = bidders,
                Images = images,
                AuctionType = auction.AuctionType,
            };
        }

        public async Task<EditAuctionInputModel> PrepareEditAuctionInputModel(Data.Models.Auction auction)
        {
            var categories = await this.categoryService
                .GetAllCategories<CategoryListModel>()
                .ToListAsync();

            var model = new EditAuctionInputModel()
            {
                Id = auction.Id,
                AuctionType = auction.AuctionType,
                Categories = categories,
                Condition = auction.Condition,
                Description = auction.Description,
                StartPrice = (int)auction.StartPrice,
                StepAmount = auction.StepAmount,
                Title = auction.Title,
                SubCateogoryId = auction.SubCategoryId,
            };

            return model;
        }

        public async Task<MyAuctionsPaginatedViewModel> PrepareMyAuctionsPaginatedViewModelAsync(int pageIndex, string userId, int itemsPerPage)
        {
            var myAuctions = await this.auctionService
                .GetAuctionsByOwnerId<MyAuctionsViewModel>(pageIndex, userId, itemsPerPage)
                .OrderBy(x => x.Status)
                .ToListAsync();

            var model = new MyAuctionsPaginatedViewModel()
            {
                ItemsCount = this.auctionService.MyAuctionsWithDeletedCount(userId),
                ActiveAuctionsCount = this.auctionService.MyActiveAuctionsCount(userId),
                PageNumber = pageIndex,
                ItemsPerPage = itemsPerPage,
                Auctions = myAuctions,
            };

            return model;
        }
    }
}
