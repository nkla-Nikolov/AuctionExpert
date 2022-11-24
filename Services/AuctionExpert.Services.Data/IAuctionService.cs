namespace AuctionExpert.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Auction;

    public interface IAuctionService
    {
        Task CreateAsync(AddAuctionViewModel model, ApplicationUser user);

        Task DeteleAsync(int auctionId);

        IQueryable<T> GetAllAuctions<T>();

        Task<DetailViewModel> GetDetailAuctionModelByIdAsync(int auctionId);

        IQueryable<T> GetAuctionsByOwnerId<T>(string ownerId);

        Task<T> GetAuctionById<T>(int auctionId);

        Task PlaceBidAsync(int? currentBid, string userId, int auctionId);

        IQueryable<T> GetAllAuctionsByCategoryId<T>(int categoryId);
    }
}
