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
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public ReviewService(
            IDeletableEntityRepository<Auction> auctionRepository,
            IDeletableEntityRepository<Review> reviewRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.auctionRepository = auctionRepository;
            this.reviewRepository = reviewRepository;
            this.userRepository = userRepository;
        }

        public IQueryable<T> GetAllReviewsOnAuction<T>(int auctionId)
        {
            return this.reviewRepository
                .AllAsNoTracking()
                .Where(x => x.AuctionId == auctionId)
                .To<T>();
        }

        public IQueryable<T> GetAllReviewsOnUser<T>(string userId)
        {
            return this.reviewRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .To<T>();
        }

        public async Task CommentOnUser(string userId, string reviewerId, string comment)
        {
            var user = await this.userRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == userId);

            var reviewer = await this.userRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.Id == reviewerId);

            if (user == null || reviewer == false)
            {
                throw new ArgumentNullException();
            }

            user.Reviews.Add(new Review()
            {
                DatePlaced = DateTime.UtcNow,
                Comment = comment,
                ReviewerId = reviewerId,
                UserId = userId,
            });

            await this.userRepository.SaveChangesAsync();
        }

        public async Task CommentOnAuction(int auctionId, string comment, string reviewerId)
        {
            var auction = await this.auctionRepository
                .All()
                .Where(x => x.Id == auctionId)
                .FirstOrDefaultAsync();

            var reviewer = await this.userRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.Id == reviewerId);

            if (auction == null || reviewer == false)
            {
                throw new NullReferenceException();
            }

            auction.Reviews.Add(new Review()
            {
                DatePlaced = DateTime.UtcNow,
                Comment = comment,
                ReviewerId = reviewerId,
                AuctionId = auctionId,
            });

            await this.auctionRepository.SaveChangesAsync();
        }
    }
}
