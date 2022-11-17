namespace AuctionExpert.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Auction;

    public interface IAuctionService
    {
        Task CreateAsync(AddAuctionViewModel model, ApplicationUser user);

        Task<List<HomeAuctionViewModel>> GetAllAuctionsAsHomeModel();

        Task<DetailViewModel> GetDetailAuctionModelByIdAsync(int auctionId);

        Task<Auction> GetAuctionById(int auctionId);

        Task PlaceBidAsync(int currentBid, string userId, int auctionId);
    }
}
