namespace AuctionExpert.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class BlogPostReview : BaseDeletableModel<int>
    {
        [Required]
        public string Comment { get; set; }

        [Required]
        public DateTime DatePlaced { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public int BlogPostId { get; set; }

        public BlogPost BlogPost { get; set; }
    }
}
