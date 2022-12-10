namespace AuctionExpert.Web.ViewModels.Administration.Country
{
    using System.Collections.Generic;

    using AuctionExpert.Web.ViewModels.Shared;

    public class PaginatedCountryListModel : PagingViewModel
    {
        public IEnumerable<AdminCountryListModel> Countries { get; set; }
    }
}
