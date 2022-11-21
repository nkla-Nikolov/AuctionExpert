namespace AuctionExpert.Services.Data
{
    using System.Linq;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class BidService : IBidService
    {
        private readonly IDeletableEntityRepository<Bid> bidRepository;

        public BidService(IDeletableEntityRepository<Bid> bidRepository)
        {
            this.bidRepository = bidRepository;
        }

        public IQueryable<T> GetAllBidsByAuctionId<T>(int auctionId)
        {
            return this.bidRepository
                .AllAsNoTracking()
                .Where(x => x.AuctionId == auctionId)
                .OrderByDescending(x => x.MoneyPlaced)
                .To<T>();
        }
    }
}
