namespace AuctionExpert.Web.ViewModels.Administration.Country
{
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class AdminCountryListModel : IMapFrom<Country>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CitiesCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Country, Country>();
        }
    }
}
