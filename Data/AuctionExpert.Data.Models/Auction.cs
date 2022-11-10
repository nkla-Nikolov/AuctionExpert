namespace AuctionExpert.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Enumerations;
    using AuctionExpert.Data.Common.Models;

    public class Auction : BaseDeletableModel<int>
    {
        public Auction()
        {
            this.Bids = new HashSet<Bid>();
        }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public int Views { get; set; }

        [Required]
        public decimal StartPrice { get; set; }

        [Required]
        public TypeSale AuctionType { get; set; }

        [Required]
        [MaxLength(10)]
        public int Duration { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
    }
}
