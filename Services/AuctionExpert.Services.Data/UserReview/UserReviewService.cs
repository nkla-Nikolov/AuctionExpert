namespace AuctionExpert.Services.Data.UserReview
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Services.Mapping;

    public class UserReviewService : IUserReviewService
    {
        private readonly IDeletableEntityRepository<UserReview> reviewRepository;
        private readonly IUserService userService;

        public UserReviewService(
            IDeletableEntityRepository<UserReview> reviewRepository,
            IUserService userService)
        {
            this.reviewRepository = reviewRepository;
            this.userService = userService;
        }

        public async Task AddCommentOnUser(string comment, string userId, string authorId)
        {
            var author = await this.userService.GetUserByIdAsync(authorId);
            var user = await this.userService.GetUserByIdAsync(userId);

            if (user == null || author == null)
            {
                throw new ArgumentNullException();
            }

            var entity = new UserReview
            {
                DatePlaced = DateTime.UtcNow,
                AuthorId = authorId,
                UserId = userId,
                Comment = comment,
            };

            await this.reviewRepository.AddAsync(entity);
            await this.reviewRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAllUserReviewsByUserIdAsync<T>(string userId)
        {
            return this.reviewRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .To<T>();
        }
    }
}
