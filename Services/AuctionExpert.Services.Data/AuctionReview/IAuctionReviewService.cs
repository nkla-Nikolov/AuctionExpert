namespace AuctionExpert.Services.Data.Review
{
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;

    public interface IAuctionReviewService
    {
        Task CommentOnAuction(Auction auction, string comment, string userId);

        IQueryable<T> GetAllAuctionReviews<T>(Auction auction);
    }
}
