namespace AuctionExpert.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Web.ViewModels.Administration.User;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using static AuctionExpert.Common.GlobalConstants;

    public class UsersController : AdministrationController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            const int itemsPerPage = 50;

            var users = await this.userService
                .GetAllUsersPaginated<UsersListViewModel>(id, itemsPerPage)
                .ToListAsync();

            var model = new PaginatedUsersListModel()
            {
                PageNumber = id,
                ItemsPerPage = itemsPerPage,
                ItemsCount = this.userService.GetCount(),
                Users = users,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userId)
        {
            try
            {
                await this.userService.RemoveUserFromRole(userId, AdministratorRoleName);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(string userId)
        {
            try
            {
                await this.userService.AddUserToRole(userId, AdministratorRoleName);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
