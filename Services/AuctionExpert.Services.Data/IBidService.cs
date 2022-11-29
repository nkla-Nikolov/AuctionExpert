namespace AuctionExpert.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBidService
    {
        IQueryable<T> GetAllBidsByAuctionId<T>(int auctionId);

        Task<decimal> HighestBidByAuctionId(int auctionId);
    }
}
