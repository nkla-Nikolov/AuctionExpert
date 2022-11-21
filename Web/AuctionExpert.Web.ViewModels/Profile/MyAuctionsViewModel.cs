﻿namespace AuctionExpert.Web.ViewModels.Profile
{
    using System.Linq;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class MyAuctionsViewModel : IMapFrom<Auction>, IHaveCustomMappings
    {
        public string ImageUrl { get; set; }

        public decimal StartPrice { get; set; }

        public decimal HighestBid { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Auction, MyAuctionsViewModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(x => x.Images.First().UrlPath))
                .ForMember(dest => dest.HighestBid, opt => opt.MapFrom(x => x.Bids.Count == 0 ?
                x.StartPrice : x.Bids.OrderByDescending(x => x.MoneyPlaced).First().MoneyPlaced))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(x => x.AuctionType.ToString()
                .StartsWith("Fixed") ? "Fixed Price Auction" : "Standard Auction"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.IsDeleted == true ? "Finished" : "Active"));
        }
    }
}
