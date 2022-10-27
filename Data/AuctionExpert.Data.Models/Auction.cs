namespace AuctionExpert.Data.Models
{
    using System.Collections.Generic;

    using AuctionExpert.Data.Common.Models;

    public class Auction : BaseDeletableModel<int>
    {
        public Auction()
        {
            this.Bids = new HashSet<Bid>();
        }

        public string Name { get; set; }

        public int Views { get; set; }

        public int StartPrice { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public int SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
    }
}
