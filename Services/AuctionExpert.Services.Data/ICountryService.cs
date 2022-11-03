namespace AuctionExpert.Services.Data
{
    using System.Linq;

    using AuctionExpert.Services.Data.Models;

    public interface ICountryService
    {
        IQueryable<CountryListModel> GetCountries();
    }
}
