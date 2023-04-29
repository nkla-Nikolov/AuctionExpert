namespace AuctionExpert.Web.ViewModels.Country
{
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class CountryListModel : IMapTo<Country>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Country, CountryListModel>();
            configuration.CreateMap<Country, Country>();
        }
    }
}
