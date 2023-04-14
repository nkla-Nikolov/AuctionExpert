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

        Task LikeAuction(Auction auction, ApplicationUser user);

        Task<Auction> GetAuctionByIdAsync(int auctionId);

        Task PlaceBidAsync(int? currentBid, string userId, int auctionId);

        Task EditAuction(int auctionId, EditAuctionInputModel model);

        IQueryable<T> GetAllAuctions<T>();

        IQueryable<T> GetAllAuctionsByCountryId<T>(int countryId);

        IQueryable<T> GetAuctionsByOwnerId<T>(int page, string ownerId, int itemsPerPage);

        IQueryable<T> GetAllAuctionsByCategoryId<T>(int page, int itemsPerPage, int categoryId);

        IQueryable<T> GetAllPaginatedAuctions<T>(int page, int itemsPerPage);

        int AllAuctionsCount();

        int AllAuctionsInCategoryCount(int categoryId);

        int MyAuctionsWithDeletedCount(string ownerId);

        int MyActiveAuctionsCount(string ownerId);

        IQueryable<T> GetAllCommentsByAuctionId<T>(int auctionId);
    }
}
