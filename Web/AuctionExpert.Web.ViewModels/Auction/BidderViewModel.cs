namespace AuctionExpert.Web.ViewModels.Auction
{
    using System;

    public class BidderViewModel
    {
        public string ProfileImageUrl { get; set; }

        public string Username { get; set; }

        public decimal MoneyPlaced { get; set; }

        public TimeSpan TimePlaced { get; set; }
    }
}
