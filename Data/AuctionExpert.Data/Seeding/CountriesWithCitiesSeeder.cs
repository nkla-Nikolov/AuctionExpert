namespace AuctionExpert.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Common;
    using AuctionExpert.Data.Models;
    using Newtonsoft.Json;

    using static AuctionExpert.Common.FilePaths;

    public class CountriesWithCitiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Countries.Any())
            {
                return;
            }

            var path = Path.Combine(
                DirectoryUp,
                DirectoryUp,
                DirectoryUp,
                GlobalConstants.SystemName,
                DataFolder,
                SystemNameData,
                FilesFolder,
                CountriesWithCitiesFileName);

            var jsonFile = await File.ReadAllTextAsync(path);

            var countriesJson = JsonConvert.DeserializeObject<SortedDictionary<string, string[]>>(jsonFile).Skip(2);
            var countries = new HashSet<Country>();

            foreach (var countryJson in countriesJson)
            {
                var country = new Country()
                {
                    Name = countryJson.Key,
                };

                foreach (var cityJson in countryJson.Value)
                {
                    var city = new City()
                    {
                        Name = cityJson,
                    };

                    country.Cities.Add(city);
                }

                countries.Add(country);
            }

            await dbContext.Countries.AddRangeAsync(countries);
            await dbContext.SaveChangesAsync();
        }
    }
}
