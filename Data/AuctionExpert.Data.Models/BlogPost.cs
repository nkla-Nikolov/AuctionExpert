namespace AuctionExpert.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class BlogPost : BaseDeletableModel<int>
    {
        public BlogPost()
        {
            this.Reviews = new List<BlogPostReview>();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        [Required]
        public string Body { get; set; }

        public ICollection<BlogPostReview> Reviews { get; set; }
    }
}
