namespace AuctionExpert.Services.Data.Review
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IReviewService
    {
        Task CommentOnAuction(int auctionId, string comment, string userId);

        IQueryable<T> GetAllCommentsOnAuction<T>(int auctionId);
    }
}
