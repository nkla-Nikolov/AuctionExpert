namespace AuctionExpert.Web.ViewModels.Administration.User
{
    using System.Collections.Generic;

    using AuctionExpert.Web.ViewModels.Shared;

    public class PaginatedUsersListModel : PagingViewModel
    {
        public IEnumerable<UsersListViewModel> Users { get; set; }
    }
}
