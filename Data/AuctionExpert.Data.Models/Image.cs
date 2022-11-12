namespace AuctionExpert.Data.Models
{
    using System;

    using AuctionExpert.Data.Common.Models;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string UrlPath { get; set; }

        public int AuctionId { get; set; }

        public virtual Auction Auction { get; set; }
    }
}
