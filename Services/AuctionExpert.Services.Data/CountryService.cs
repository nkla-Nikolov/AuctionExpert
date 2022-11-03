namespace AuctionExpert.Services.Data
{
    using System.Linq;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Models;

    public class CountryService : ICountryService
    {
        private readonly IDeletableEntityRepository<Country> countryRepository;

        public CountryService(IDeletableEntityRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public IQueryable<CountryListModel> GetCountries()
        {
            return this.countryRepository
                .AllAsNoTracking()
                .Select(x => new CountryListModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                });
        }
    }
}
