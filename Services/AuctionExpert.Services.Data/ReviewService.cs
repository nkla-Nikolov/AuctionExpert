namespace AuctionExpert.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class ReviewService : IReviewService
    {
        private readonly IAuctionService auctionService;
        private readonly IDeletableEntityRepository<Auction> auctionRepository;
        private readonly IDeletableEntityRepository<Review> reviewRepository;

        public ReviewService(
            IAuctionService auctionService,
            IDeletableEntityRepository<Auction> auctionRepository,
            IDeletableEntityRepository<Review> reviewRepository)
        {
            this.auctionService = auctionService;
            this.auctionRepository = auctionRepository;
            this.reviewRepository = reviewRepository;
        }

        public async Task CommentOnAuction(int auctionId, string comment, string userId)
        {
            var auction = await this.auctionService.GetAuctionById<Auction>(auctionId);

            if (auction == null)
            {
                throw new NullReferenceException();
            }

            auction.Reviews.Add(new Review()
            {
                DatePlaced = DateTime.UtcNow,
                Comment = comment,
                UserId = userId,
            });

            this.auctionRepository.Update(auction);
            await this.auctionRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAllCommentsOnAuction<T>(int auctionId)
        {
            return this.reviewRepository
                .AllAsNoTracking()
                .Where(x => x.AuctionId == auctionId)
                .To<T>();
        }
    }
}
