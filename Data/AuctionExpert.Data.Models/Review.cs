namespace AuctionExpert.Data.Models
{
    using System;

    using AuctionExpert.Data.Common.Models;

    public class Review : BaseDeletableModel<int>
    {
        public DateTime? CommentDate { get; set; }

        public string Comment { get; set; }

        public int PositiveReviews { get; set; }

        public int NegativeReviews { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
