namespace AuctionExpert.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Web.ViewModels.Administration.User;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using NToastNotify;

    using static AuctionExpert.Common.GlobalConstants;

    public class UserController : AdministrationController
    {
        private readonly IUserService userService;
        private readonly IToastNotification notificationService;

        public UserController(
            IUserService userService,
            IToastNotification notificationService)
        {
            this.userService = userService;
            this.notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult All()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAll(int itemsPerPage, int currentPage)
        {
            var users = await this.userService
                .GetAllUsers<UsersListViewModel>()
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToArrayAsync();

            var usersCount = users.Length;
            var pagesCount = (int)Math.Ceiling((double)usersCount / itemsPerPage);

            var model = new
            {
                PagesCount = pagesCount,
                ItemsPerPage = itemsPerPage,
                HasPreviousPage = currentPage > 1,
                HasNextPage = currentPage < pagesCount,
                CurrentPage = currentPage,
                Users = users,
                Success = true,
            };

            return this.Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBySearchTerm(string searchTerm)
        {
            var users = await this.userService
                .GetAllUsers<UsersListViewModel>()
                .Where(x => x.Email.Contains(searchTerm) || x.Username.Contains(searchTerm))
                .ToListAsync();

            return this.Json(users);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userId)
        {
            var user = await this.userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                this.notificationService.AddErrorToastMessage("User does not exist");
                return this.Json(new { Success = false });
            }

            try
            {
                await this.userService.RemoveUserFromRole(user, AdministratorRoleName);
            }
            catch (ArgumentNullException)
            {
                this.notificationService.AddErrorToastMessage($"Role {AdministratorRoleName} does not exist");
                return this.Json(new { Success = false });
            }

            this.notificationService.AddSuccessToastMessage($"Successfully removed {user.UserName} from administrators");
            return this.Json(new { Success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(string userId)
        {
            var user = await this.userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                this.notificationService.AddErrorToastMessage("User does not exist");
                return this.Json(new { Success = false });
            }

            try
            {
                await this.userService.AddUserToRole(user, AdministratorRoleName);
            }
            catch (ArgumentNullException)
            {
                this.notificationService.AddErrorToastMessage($"Role {AdministratorRoleName} does not exist");
                return this.Json(new { Success = false });
            }

            this.notificationService.AddSuccessToastMessage($"Successfully added {user.UserName} to administrators");
            return this.Json(new { Success = true });
        }
    }
}
