namespace AuctionExpert.Services.Data.Country
{
    using System.Linq;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class CountryService : ICountryService
    {
        private readonly IDeletableEntityRepository<Country> countryRepository;

        public CountryService(IDeletableEntityRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public IQueryable<T> GetAllCountries<T>()
        {
            return countryRepository
                .AllAsNoTracking()
                .To<T>();
        }
    }
}
