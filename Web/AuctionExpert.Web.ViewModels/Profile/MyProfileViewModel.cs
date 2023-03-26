namespace AuctionExpert.Web.ViewModels.Profile
{
    public class MyProfileViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string CityName { get; set; }

        public string ProfileImageUrl { get; set; }

        public string Username { get; set; }

        public UpdateProfileViewModel UpdateProfileInput { get; set; }

        public int? CityId { get; set; }
    }
}
