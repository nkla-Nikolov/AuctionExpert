namespace AuctionExpert.Services.Data.User
{
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Web.ViewModels.Profile;

    public interface IUserService
    {
        Task UpdateProfile(MyProfileViewModel model, string userId);

        IQueryable<T> GetAllUsers<T>();

        IQueryable<T> GetAllUsersPaginated<T>(int page, int itemsPerPage = 50);

        int GetCount();

        Task AddUserToRole(string userId, string roleName);

        Task RemoveUserFromRole(string userId, string roleName);
    }
}
