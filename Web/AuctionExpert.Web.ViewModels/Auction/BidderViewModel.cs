namespace AuctionExpert.Web.ViewModels.Auction
{
    using System;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class BidderViewModel : IMapFrom<Bid>, IHaveCustomMappings
    {
        public string BidderId { get; set; }

        public string ProfileImageUrl { get; set; }

        public string Username { get; set; }

        public decimal MoneyPlaced { get; set; }

        public TimeSpan TimePlaced { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Bid, BidderViewModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(x => x.Bidder.UserName))
                .ForMember(dest => dest.TimePlaced, opt => opt.MapFrom(x => DateTime.UtcNow - x.TimePlaced));
        }
    }
}
