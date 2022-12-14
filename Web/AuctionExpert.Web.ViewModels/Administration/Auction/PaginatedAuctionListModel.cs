namespace AuctionExpert.Web.ViewModels.Administration.Auction
{
    using System.Collections.Generic;

    using AuctionExpert.Web.ViewModels.Shared;

    public class PaginatedAuctionListModel : PagingViewModel
    {
        public IEnumerable<AdminAuctionViewModel> Auctions { get; set; }
    }
}
