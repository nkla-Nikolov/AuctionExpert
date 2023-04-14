namespace AuctionExpert.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class UserReview : BaseDeletableModel<int>
    {
        [Required]
        public string Comment { get; set; }

        public int Likes { get; set; }

        [Required]
        public DateTime DatePlaced { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}
