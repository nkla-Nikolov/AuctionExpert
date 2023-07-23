namespace AuctionExpert.Web.ViewModels.Administration.Auction
{
    using System.Collections.Generic;

    using AuctionExpert.Web.ViewModels.Shared;

    public class PaginatedAuctionListModel : BasePageModel
    {
        public IEnumerable<AuctionModel> Auctions { get; set; }
    }
}
