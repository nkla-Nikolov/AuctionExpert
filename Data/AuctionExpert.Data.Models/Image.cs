namespace AuctionExpert.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Models;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string UrlPath { get; set; }

        [Required]
        public int AuctionId { get; set; }

        public virtual Auction Auction { get; set; }
    }
}
