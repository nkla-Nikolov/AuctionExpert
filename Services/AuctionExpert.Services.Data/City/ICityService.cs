namespace AuctionExpert.Services.Data.City
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICityService
    {
        IQueryable<T> GetAllCitiesByCountryId<T>(int countryId);

        Task<T> GetCityById<T>(int? cityId);
    }
}
