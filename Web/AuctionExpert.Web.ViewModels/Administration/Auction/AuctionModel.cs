namespace AuctionExpert.Web.ViewModels.Administration.Auction
{
    using System;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class AuctionModel : IMapFrom<Auction>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int BidsCount { get; set; }

        public string OwnerId { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ClosesIn { get; set; }
    }
}
