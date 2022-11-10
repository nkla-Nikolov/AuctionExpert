namespace AuctionExpert.Services.Data
{
    using System.Linq;

    public interface ICountriesService
    {
        IQueryable<T> GetAllCountries<T>();
    }
}
