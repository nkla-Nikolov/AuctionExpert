using AuctionExpert.Web.ViewModels.Profile;

namespace AuctionExpert.Factories.User
{
    public interface IUserModelFactory
    {
        public Task<MyProfileViewModel> PrepareMyProfileViewModelAsync(string userId);

        public Task<UpdateProfileViewModel> PrepareUpdateProfileViewModelAsync(int countryId);

        public Task<SellerProfileViewModel> PrepareSellerProfileViewModelAsync(string userId);
    }
}
