namespace AuctionExpert.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class City : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}
