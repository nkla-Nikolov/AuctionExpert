namespace AuctionExpert.Data.Models
{
    using System;

    using AuctionExpert.Data.Common.Enumerations;
    using AuctionExpert.Data.Common.Models;

    public class Product : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public DateTime? DateOfProduction { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Weight { get; set; }

        public ConditionType Condition { get; set; }

        public string Description { get; set; }
    }
}
