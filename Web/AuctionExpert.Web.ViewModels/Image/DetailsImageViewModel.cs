namespace AuctionExpert.Web.ViewModels.Image
{
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class DetailsImageViewModel : IMapFrom<Image>, IHaveCustomMappings
    {
        public string UrlPath { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Image, DetailsImageViewModel>();
        }
    }
}
