namespace AuctionExpert.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICityService
    {
        IQueryable<T> GetAllCitiesByCountryId<T>(int countryId);

        Task<T> GetCityById<T>(int? cityId);
    }
}
