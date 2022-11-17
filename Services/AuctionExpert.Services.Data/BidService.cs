namespace AuctionExpert.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Bid;
    using Microsoft.EntityFrameworkCore;

    public class BidService : IBidService
    {
        private readonly IDeletableEntityRepository<Bid> bidRepository;

        public BidService(IDeletableEntityRepository<Bid> bidRepository)
        {
            this.bidRepository = bidRepository;
        }

        public async Task<List<BidListModel>> GetAllBidsByAuctionIdAsync(int auctionId)
        {
            return await this.bidRepository
                .AllAsNoTracking()
                .Where(x => x.AuctionId == auctionId)
                .Select(x => new BidListModel()
                {
                    MoneyPlaced = x.MoneyPlaced,
                })
                .OrderByDescending(x => x.MoneyPlaced)
                .ToListAsync();
        }
    }
}
