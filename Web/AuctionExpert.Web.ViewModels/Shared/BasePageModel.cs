namespace AuctionExpert.Web.ViewModels.Shared
{
    using System;

    public abstract class BasePageModel
    {
        public int PagesCount => (int)Math.Ceiling((double)this.ItemsCount / this.ItemsPerPage);

        public int ItemsPerPage { get; set; }

        public int ItemsCount { get; set; }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int NextPageNumber => this.PageNumber + 1;

        public string AspAction { get; set; }
    }
}
