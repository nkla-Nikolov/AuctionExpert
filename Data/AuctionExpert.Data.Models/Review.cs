namespace AuctionExpert.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class Review : BaseDeletableModel<int>
    {
        public string Comment { get; set; }

        public int PositiveReviews { get; set; }

        public int NegativeReviews { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
