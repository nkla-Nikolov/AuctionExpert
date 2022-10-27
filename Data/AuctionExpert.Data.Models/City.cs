namespace AuctionExpert.Data.Models
{
    using System.Collections.Generic;

    using AuctionExpert.Data.Common.Models;

    public class City : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}
