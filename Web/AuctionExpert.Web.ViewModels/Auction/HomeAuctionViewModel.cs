namespace AuctionExpert.Web.ViewModels.Auction
{
    using System;

    public class HomeAuctionViewModel
    {
        public string Title { get; set; }

        public string MainImage { get; set; }

        public int Views { get; set; }

        public string AuthorName { get; set; }

        public decimal LastBid { get; set; }

        public DateTime ClosesIn { get; set; }
    }
}
