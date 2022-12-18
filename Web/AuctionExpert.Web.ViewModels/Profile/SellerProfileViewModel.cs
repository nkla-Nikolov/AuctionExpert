namespace AuctionExpert.Web.ViewModels.Profile
{
    using System.Collections.Generic;

    using AuctionExpert.Web.ViewModels.Review;

    public class SellerProfileViewModel
    {
        public SellerProfileViewModel()
        {
            this.Comments = new HashSet<ReviewViewModel>();
        }

        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string AvatarUrl { get; set; }

        public string Comment { get; set; }

        public IEnumerable<ReviewViewModel> Comments { get; set; }
    }
}
