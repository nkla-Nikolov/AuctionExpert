namespace AuctionExpert.Services.Data.Auction
{
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Auction;

    public interface IAuctionService
    {
        Task CreateAsync(AddAuctionViewModel model, ApplicationUser user);

        Task DeleteAsync(int auctionId);

        IQueryable<T> GetAllAuctions<T>();

        IQueryable<T> GetAllAuctionsByCountryId<T>(int countryId);

        Task<DetailViewModel> GetDetailAuctionModelByIdAsync(int auctionId);

        IQueryable<T> GetAuctionsByOwnerId<T>(string ownerId);

        Task<T> GetAuctionById<T>(int auctionId);

        Task PlaceBidAsync(int? currentBid, string userId, Auction auction);

        IQueryable<T> GetAllAuctionsByCategoryId<T>(int categoryId);
    }
}
