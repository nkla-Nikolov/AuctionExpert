using AuctionExpert.Web.ViewModels.Auction;

namespace AuctionExpert.Factories.Auction
{
    public interface IAuctionModelFactory
    {
        Task<MyAuctionsPaginatedViewModel> PrepareMyAuctionsPaginatedViewModelAsync(int pageIndex, string userId, int itemsPerPage);

        Task<DetailViewModel> PrepareAuctionDetailViewModelAsync(int auctionId);

        Task<EditAuctionInputModel> PrepareEditAuctionInputModel(Data.Models.Auction auction);
    }
}
