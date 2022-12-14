namespace AuctionExpert.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AuctionExpert.Services.Data.Country;
    using AuctionExpert.Web.ViewModels.Administration.Country;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class CountriesController : AdministrationController
    {
        private readonly ICountryService countryService;

        public CountriesController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            const int itemsPerPage = 50;

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

        [HttpPost]
        public async Task<IActionResult> Delete(int countryId)
        {
            try
            {
                await this.countryService.DeleteCountry(countryId);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            return this.RedirectToAction(nameof(this.All));
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
