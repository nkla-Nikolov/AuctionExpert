﻿namespace AuctionExpert.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.SubCategories = new HashSet<SubCategory>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
