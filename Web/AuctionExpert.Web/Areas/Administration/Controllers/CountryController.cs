namespace AuctionExpert.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Country;
    using AuctionExpert.Web.ViewModels.Administration.Country;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using NToastNotify;

    public class CountryController : AdministrationController
    {
        private readonly ICountryService countryService;
        private readonly IToastNotification notificationService;

        public CountryController(
            ICountryService countryService,
            IToastNotification notificationService)
        {
            this.countryService = countryService;
            this.notificationService = notificationService;
        }

        public IActionResult All()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAll(int currentPage, int itemsPerPage)
        {
            var countries = await this.countryService
                .GetAllCountries<CountryListModel>()
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();

            var countriesCount = this.countryService.GetCount();

            var pagesCount = (int)Math.Ceiling((double)countriesCount / itemsPerPage);
            var model = new
            {
                PagesCount = pagesCount,
                ItemsPerPage = itemsPerPage,
                HasPreviousPage = currentPage > 1,
                HasNextPage = currentPage < pagesCount,
                Countries = countries,
                CurrentPage = currentPage,
            };

            return this.Json(model);
        }

        public async Task<IActionResult> GetAllBySearchTerm(string name)
        {
            var countries = await this.countryService
                .GetAllCountries<CountryListModel>()
                .Where(x => x.Name.Contains(name))
                .ToListAsync();

            return this.Json(countries);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int countryId)
        {
            try
            {
                await this.countryService.DeleteCountry(countryId);
            }
            catch (ArgumentNullException)
            {
                return this.Json(new { Result = false });
            }

            this.notificationService.AddSuccessToastMessage($"Country deleted successfully");
            return this.Json(new { Result = true });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int countryId, string countryName)
        {
            var country = await this.countryService
                .GetCountryById<Country>(countryId);

            if (country == null)
            {
                return this.Json(new { Result = false, Message = "Country does not exist!" });
            }

            if (string.IsNullOrEmpty(countryName) || countryName.Length < 3)
            {
                return this.Json(new { Result = false, Message = "Country name should be more than 3 symbols!" });
            }

            await this.countryService.UpdateCountry(country, countryName);
            return this.Json(new { Result = true });
        }

        [HttpPost]
        public async Task<IActionResult> Add(string countryName)
        {
            if (string.IsNullOrEmpty(countryName))
            {
                return this.Json(new { Success = false, ErrorMessage = "Name should not be empty!" });
            }

            var country = new Country
            {
                Name = countryName,
                CreatedOn = DateTime.UtcNow,
            };
            await this.countryService.AddCountry(country);

            this.notificationService.AddSuccessToastMessage("Country added successfully!");
            return this.Json(new { Success = true });
        }
    }
}
