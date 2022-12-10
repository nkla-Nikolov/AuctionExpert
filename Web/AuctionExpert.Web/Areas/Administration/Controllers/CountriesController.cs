namespace AuctionExpert.Web.Areas.Administration.Controllers
{
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
                .GetAllCountriesPaginated<AdminCountryListModel>(id, itemsPerPage)
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
    }
}
