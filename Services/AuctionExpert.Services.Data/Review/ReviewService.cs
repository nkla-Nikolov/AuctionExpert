namespace AuctionExpert.Services.Data.Review
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ReviewService : IReviewService
    {
        private readonly IDeletableEntityRepository<Auction> auctionRepository;
        private readonly IDeletableEntityRepository<Review> reviewRepository;

        public ReviewService(
            IDeletableEntityRepository<Auction> auctionRepository,
            IDeletableEntityRepository<Review> reviewRepository)
        {
            this.auctionRepository = auctionRepository;
            this.reviewRepository = reviewRepository;
        }

        public async Task CommentOnAuction(int auctionId, string comment, string userId)
        {
            var auction = await auctionRepository
                .AllAsNoTracking()
                .Where(x => x.Id == auctionId)
                .FirstOrDefaultAsync();

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

            auctionRepository.Update(auction);
            await auctionRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAllCommentsOnAuction<T>(int auctionId)
        {
            return reviewRepository
                .AllAsNoTracking()
                .Where(x => x.AuctionId == auctionId)
                .To<T>();
        }
    }
}
