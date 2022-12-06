namespace AuctionExpert.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class Review : BaseDeletableModel<int>
    {
        [Required]
        public string Comment { get; set; }

        public int PositiveReviews { get; set; }

        public int NegativeReviews { get; set; }

        [Required]
        public DateTime DatePlaced { get; set; }

        public int? AuctionId { get; set; }

        public virtual Auction Auction { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ReviewerId { get; set; }

        public virtual ApplicationUser Reviewer { get; set; }
    }
}
