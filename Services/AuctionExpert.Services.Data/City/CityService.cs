namespace AuctionExpert.Services.Data.City
{
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<T> GetCityById<T>(int? cityId)
        {
            return await this.cityRepository
                .AllAsNoTracking()
                .Where(x => x.Id == cityId)
                .To<T>()
                .FirstOrDefaultAsync();
        }
    }
}
