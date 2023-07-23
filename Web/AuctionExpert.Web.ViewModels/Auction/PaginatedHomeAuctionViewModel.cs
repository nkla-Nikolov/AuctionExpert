namespace AuctionExpert.Web.ViewModels.Auction
{
    using System.Collections.Generic;

    using AuctionExpert.Web.ViewModels.Shared;

    public class PaginatedHomeAuctionViewModel : BasePageModel
    {
        public IEnumerable<HomeAuctionViewModel> Auctions { get; set; }

        public int CategoryId { get; set; }
    }
}
