namespace AuctionExpert.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Country;
    using AuctionExpert.Web.ViewModels.Administration.Country;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using NToastNotify;

    public class CountriesController : AdministrationController
    {
        private readonly ICountryService countryService;
        private readonly IToastNotification notificationService;

        public CountriesController(
            ICountryService countryService,
            IToastNotification notificationService)
        {
            this.countryService = countryService;
            this.notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            const int itemsPerPage = 500;

            var countries = await this.countryService
                .GetAllCountriesPaginated<CountryListModel>(id, itemsPerPage)
                .ToListAsync();

            var model = new PaginatedCountryListModel()
            {
                PageNumber = id,
                ItemsPerPage = itemsPerPage,
                ItemsCount = this.countryService.GetCount(),
                Countries = countries,
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await this.countryService
                .GetAllCountries<CountryListModel>()
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
        public async Task<IActionResult> Add(PaginatedCountryListModel model)
        {
            if (model.CountryName == null)
            {
                this.ModelState.AddModelError(nameof(model.CountryName), "To add a country you need to enter a name!");
                return this.View(model);
            }

            await this.countryService.AddCountry(model.CountryName);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
