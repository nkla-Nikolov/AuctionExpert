namespace AuctionExpert.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class SubCategory : BaseDeletableModel<int>
    {
        public SubCategory()
        {
            this.Auctions = new HashSet<Auction>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Auction> Auctions { get; set; }
    }
}
