namespace AuctionExpert.Services.Data.Country
{
    using System.Linq;

    public interface ICountryService
    {
        IQueryable<T> GetAllCountries<T>();
    }
}
