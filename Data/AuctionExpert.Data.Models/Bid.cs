namespace AuctionExpert.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class Bid : BaseDeletableModel<int>
    {
        [Required]
        public decimal MoneyPlaced { get; set; }

        [Required]
        public DateTime TimePlaced { get; set; }

        [Required]
        public int AuctionId { get; set; }

        public virtual Auction Auction { get; set; }

        [Required]
        public string BidderId { get; set; }

        public virtual ApplicationUser Bidder { get; set; }
    }
}
