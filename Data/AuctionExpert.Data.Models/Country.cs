namespace AuctionExpert.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class Country : BaseDeletableModel<int>
    {
        public Country()
        {
            this.Cities = new HashSet<City>();
            this.Users = new HashSet<ApplicationUser>();
            this.Auctions = new HashSet<Auction>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Auction> Auctions { get; set; }
    }
}
