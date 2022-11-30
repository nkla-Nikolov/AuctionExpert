namespace AuctionExpert.Services.Data.Bid
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBidService
    {
        IQueryable<T> GetAllBidsByAuctionId<T>(int auctionId);

        Task<decimal> GetLastHighestBid(int auctionId);
    }
}
