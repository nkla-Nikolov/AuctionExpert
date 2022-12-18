namespace AuctionExpert.Services.Data.Country
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CountryService : ICountryService
    {
        private readonly IDeletableEntityRepository<Country> countryRepository;

        public CountryService(IDeletableEntityRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public IQueryable<T> GetAllCountries<T>()
        {
            return this.countryRepository
                .AllAsNoTracking()
                .To<T>();
        }

        public async Task<T> GetCountryById<T>(int id)
        {
            return await this.countryRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAllCountriesPaginated<T>(int page, int itemsPerPage = 50)
        {
            return this.countryRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>();
        }

        public int GetCount()
        {
            return this.countryRepository
                .AllAsNoTracking()
                .Count();
        }

        public async Task DeleteCountry(int id)
        {
            var country = await this.countryRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (country == null)
            {
                throw new ArgumentNullException();
            }

            this.countryRepository.Delete(country);
            await this.countryRepository.SaveChangesAsync();
        }

        public async Task AddCountry(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            var country = new Country()
            {
                Name = name,
            };

            await this.countryRepository.AddAsync(country);
            await this.countryRepository.SaveChangesAsync();
        }
    }
}
