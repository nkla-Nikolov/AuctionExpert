﻿namespace AuctionExpert.Web.ViewModels.Auction
{
    using System.Collections.Generic;

    using AuctionExpert.Web.ViewModels.Shared;

    public class MyAuctionsPaginatedViewModel : PagingViewModel
    {
        public IEnumerable<MyAuctionsViewModel> Auctions { get; set; }

        public int ActiveAuctionsCount { get; set; }
    }
}
