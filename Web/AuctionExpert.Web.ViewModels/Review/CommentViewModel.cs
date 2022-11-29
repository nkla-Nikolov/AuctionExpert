namespace AuctionExpert.Web.ViewModels.Review
{
    using System;

    public class CommentViewModel
    {
        public DateTime DatePlaced { get; set; }

        public string Content { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }
    }
}
