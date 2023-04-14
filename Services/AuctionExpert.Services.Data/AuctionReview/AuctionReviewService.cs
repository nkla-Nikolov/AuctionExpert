namespace AuctionExpert.Services.Data.Review
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Auction;
    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Services.Mapping;

    public class AuctionReviewService : IAuctionReviewService
    {
        private readonly IDeletableEntityRepository<AuctionReview> reviewRepository;
        private readonly IUserService userService;

        public AuctionReviewService(
            IDeletableEntityRepository<AuctionReview> reviewRepository,
            IUserService userService)
        {
            this.reviewRepository = reviewRepository;
            this.userService = userService;
        }

        public IQueryable<T> GetAllAuctionReviews<T>(Auction auction)
        {
            return this.reviewRepository
                .AllAsNoTracking()
                .Where(x => x.AuctionId == auction.Id)
                .To<T>();
        }

        public async Task CommentOnAuction(Auction auction, string comment, string reviewerId)
        {
            var reviewer = await this.userService.GetUserByIdAsync(reviewerId);

            if (auction == null || reviewer == null)
            {
                throw new ArgumentNullException();
            }

            var entity = new AuctionReview
            {
                DatePlaced = DateTime.UtcNow,
                Comment = comment,
                AuthorId = reviewerId,
                AuctionId = auction.Id,
            };

            await this.reviewRepository.AddAsync(entity);
            await this.reviewRepository.SaveChangesAsync();
        }
    }
}
