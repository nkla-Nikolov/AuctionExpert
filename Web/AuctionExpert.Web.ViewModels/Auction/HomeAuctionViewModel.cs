namespace AuctionExpert.Web.ViewModels.Auction
{
    using System;
    using System.Linq;

    using AuctionExpert.Data.Common.Enumerations;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class HomeAuctionViewModel : IMapFrom<Auction>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string MainImage { get; set; }

        public int Views { get; set; }

        public string OwnerName { get; set; }

        public string OwnerId { get; set; }

        public TypeSale AuctionType { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public int ReviewsCount { get; set; }

        public decimal LastBid { get; set; }

        public DateTime ClosesIn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Auction, Auction>();

            configuration.CreateMap<Auction, HomeAuctionViewModel>()
                .ForMember(dest => dest.MainImage, opt => opt.MapFrom(x => x.Images.FirstOrDefault().UrlPath))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(x => x.Owner.FirstName))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x => x.SubCategory.Category.Name))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(x => x.SubCategory.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(x => x.SubCategory.CategoryId))
                .ForMember(dest => dest.LastBid, opt => opt.MapFrom(x => x.Bids.Count == 0 ?
                x.StartPrice : x.Bids.OrderByDescending(b => b.MoneyPlaced).First().MoneyPlaced));
        }
    }
}
