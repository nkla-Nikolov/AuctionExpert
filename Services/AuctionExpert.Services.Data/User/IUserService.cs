namespace AuctionExpert.Services.Data.User
{
    using System.Linq;
    using System.Threading.Tasks;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Profile;

    public interface IUserService
    {
        Task UpdateProfile(MyProfileViewModel model, string userId);

        IQueryable<T> GetAllUsers<T>();

        Task AddUserToRole(string userId, string roleName);

        Task<ApplicationUser> RemoveUserFromRole(string userId, string roleName);
    }
}
