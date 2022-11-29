namespace AuctionExpert.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<decimal> HighestBidByAuctionId(int auctionId)
        {
            return await this.bidRepository
                .AllAsNoTracking()
                .Where(x => x.AuctionId == auctionId)
                .OrderByDescending(x => x.MoneyPlaced)
                .Select(x => x.MoneyPlaced)
                .FirstOrDefaultAsync();
        }
    }
}
