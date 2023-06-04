namespace AuctionExpert.Services.Data.Country
{
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;

    public interface ICountryService
    {
        IQueryable<T> GetAllCountries<T>();

        Task<T> GetCountryById<T>(int id);

        IQueryable<T> GetAllCountriesPaginated<T>(int page, int itemsPerPage = 50);

        Task DeleteCountry(int id);

        Task UpdateCountry(AuctionExpert.Data.Models.Country country, string countryName);

        Task AddCountry(Country country);

        int GetCount();
    }
}
