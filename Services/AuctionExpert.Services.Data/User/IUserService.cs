namespace AuctionExpert.Services.Data.User
{
    using System.Threading.Tasks;

    using AuctionExpert.Web.ViewModels.Profile;

    public interface IUserService
    {
        Task UpdateProfile(MyProfileViewModel model, string userId);
    }
}
