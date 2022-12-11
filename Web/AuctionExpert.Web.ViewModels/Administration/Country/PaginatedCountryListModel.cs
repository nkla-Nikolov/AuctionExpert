namespace AuctionExpert.Web.ViewModels.Administration.Country
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Web.ViewModels.Shared;

    public class PaginatedCountryListModel : PagingViewModel
    {
        public IEnumerable<AdminCountryListModel> Countries { get; set; }

        [StringLength(20, ErrorMessage = "Name of the country should be less than 20 characters!")]
        public string CountryName { get; set; }
    }
}
