namespace AuctionExpert.Data.Models
{
    using System.Collections.Generic;

    using AuctionExpert.Data.Common.Models;

    public class SubCategory : BaseDeletableModel<int>
    {
        public SubCategory()
        {
            this.Auctions = new HashSet<Auction>();
        }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Auction> Auctions { get; set; }
    }
}
