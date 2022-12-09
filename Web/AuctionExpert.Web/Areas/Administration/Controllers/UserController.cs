namespace AuctionExpert.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Web.ViewModels.Administration.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using static AuctionExpert.Common.GlobalConstants;

    public class UserController : AdministrationController
    {
        private readonly IUserService userService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(
            IUserService userService,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userService = userService;
            this.signInManager = signInManager;
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
        public async Task<IActionResult> RemoveRole(string userId)
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

            await this.signInManager.RefreshSignInAsync(user);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string userId)
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

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
