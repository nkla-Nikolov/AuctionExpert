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

        Task<ApplicationUser> GetUserByIdAsync(string userId);

        IQueryable<T> GetAllUsersPaginated<T>(int page, int itemsPerPage = 50);

        int GetCount();

        Task AddUserToRole(ApplicationUser user, string roleName);

        Task RemoveUserFromRole(ApplicationUser user, string roleName);
    }
}
