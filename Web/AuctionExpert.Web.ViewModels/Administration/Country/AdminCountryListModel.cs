namespace AuctionExpert.Web.ViewModels.Administration.Country
{
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class AdminCountryListModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CitiesCount { get; set; }
    }
}
