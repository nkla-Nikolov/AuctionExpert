namespace AuctionExpert.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Profile;

    public interface IAuctionService
    {
        Task CreateAsync(AddAuctionViewModel model, ApplicationUser user);

        IQueryable<HomeAuctionViewModel> GetAllAuctionsAsHomeModel();

        Task<DetailViewModel> GetDetailAuctionModelByIdAsync(int auctionId);

        IQueryable<MyAuctionsViewModel> GetAuctionsByOwnerId(string ownerId);

        Task<Auction> GetAuctionById(int auctionId);

        Task PlaceBidAsync(int? currentBid, string userId, int auctionId);
    }
}
