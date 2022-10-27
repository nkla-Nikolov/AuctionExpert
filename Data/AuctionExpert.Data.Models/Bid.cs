namespace AuctionExpert.Data.Models
{
    using System;

    using AuctionExpert.Data.Common.Models;

    public class Bid : BaseDeletableModel<int>
    {
        public int MoneyPlaced { get; set; }

        public DateTime TimePlaced { get; set; }

        public int AuctionId { get; set; }

        public virtual Auction Auction { get; set; }

        public string BidderId { get; set; }

        public virtual ApplicationUser Bidder { get; set; }
    }
}
