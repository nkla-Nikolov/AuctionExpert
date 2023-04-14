namespace AuctionExpert.Web.ViewModels.Image
{
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class DetailsImageViewModel : IMapFrom<Image>
    {
        public string UrlPath { get; set; }
    }
}
