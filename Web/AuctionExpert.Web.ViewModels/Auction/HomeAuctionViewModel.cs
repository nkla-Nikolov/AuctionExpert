namespace AuctionExpert.Web.ViewModels.Auction
{
    using AuctionExpert.Services.Mapping;
    using System;

    public class HomeAuctionViewModel
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int Views { get; set; }

        public string AuthorName { get; set; }

        public decimal LastBid { get; set; }

        public DateTime RemainingTime { get; set; }
    }
}
