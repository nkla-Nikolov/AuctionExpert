namespace AuctionExpert.Services.Data
{
    using System.Linq;

    public interface IBidService
    {
        IQueryable<T> GetAllBidsByAuctionId<T>(int auctionId);
    }
}
