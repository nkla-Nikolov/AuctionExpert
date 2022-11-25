namespace AuctionExpert.Services.Data
{
    using System.Linq;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class CityService : ICityService
    {
        private readonly IDeletableEntityRepository<City> cityRepository;

        public CityService(IDeletableEntityRepository<City> cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        public IQueryable<T> GetAllCitiesByCountryId<T>(int countryId)
        {
            return this.cityRepository
                .AllAsNoTracking()
                .Where(x => x.CountryId == countryId)
                .To<T>();
        }
    }
}
