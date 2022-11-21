namespace AuctionExpert.Web.ViewModels.Bid
{
    using System.Collections.Generic;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class BidListModel : IMapFrom<Bid>, IHaveCustomMappings
    {
        public decimal MoneyPlaced { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<IEnumerable<Bid>, IEnumerable<BidListModel>>();
        }
    }
}
