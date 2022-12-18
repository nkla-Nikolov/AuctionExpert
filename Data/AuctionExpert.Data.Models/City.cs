namespace AuctionExpert.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class City : BaseDeletableModel<int>
    {
        public City()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
