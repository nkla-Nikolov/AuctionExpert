namespace AuctionExpert.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AuctionExpert.Web.ViewModels.Bid;

    public interface IBidService
    {
        Task<List<BidListModel>> GetAllBidsByAuctionIdAsync(int auctionId);
    }
}
