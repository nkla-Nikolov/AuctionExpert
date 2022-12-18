namespace AuctionExpert.Services.Data.Review
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IReviewService
    {
        Task CommentOnAuction(int auctionId, string comment, string userId);

        Task CommentOnUser(string userId, string reviewerId, string comment);

        IQueryable<T> GetAllReviewsOnAuction<T>(int auctionId);

        IQueryable<T> GetAllReviewsOnUser<T>(string userId);
    }
}
