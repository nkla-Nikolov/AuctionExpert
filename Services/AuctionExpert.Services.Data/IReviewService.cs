namespace AuctionExpert.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IReviewService
    {
        Task CommentOnAuction(int auctionId, string comment, string userId);

        IQueryable<T> GetAllCommentsOnAuction<T>(int auctionId);
    }
}
