namespace AuctionExpert.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Web.ViewModels.Administration.User;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using static AuctionExpert.Common.GlobalConstants;

    public class UserController : AdministrationController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.userService
                .GetAllUsers<UsersListViewModel>()
                .ToListAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userId)
        {
            ApplicationUser user = null;
            try
            {
                user = await this.userService.RemoveUserFromRole(userId, AdministratorRoleName);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            return this.RedirectToAction(nameof(this.Index));
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

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
