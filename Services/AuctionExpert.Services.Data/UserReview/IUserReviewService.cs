namespace AuctionExpert.Services.Data.UserReview
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IUserReviewService
    {
        IQueryable<T> GetAllUserReviewsByUserIdAsync<T>(string userId);

        Task AddCommentOnUser(string comment, string userId, string authorId);
    }
}
