namespace AuctionExpert.Services.Data
{
    using System.Linq;

    public interface ICityService
    {
        IQueryable<T> GetAllCitiesByCountryId<T>(int countryId);
    }
}
