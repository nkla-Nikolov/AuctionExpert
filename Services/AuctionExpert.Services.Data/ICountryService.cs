namespace AuctionExpert.Services.Data
{
    using System.Linq;

    public interface ICountryService
    {
        IQueryable<T> GetAllCountries<T>();
    }
}
