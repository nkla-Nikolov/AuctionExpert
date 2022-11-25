namespace AuctionExpert.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.City;
    using AuctionExpert.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class UserController : BaseController
    {
        private readonly IAuctionService auctionService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        private readonly ICityService cityService;

        public UserController(
            IAuctionService auctionService,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            ICityService cityService)
        {
            this.auctionService = auctionService;
            this.userManager = userManager;
            this.userService = userService;
            this.cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.userManager.FindByIdAsync(userId);

            var model = new MyProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                UpdateProfileInput = new UpdateProfileViewModel()
                {
                    Cities = await this.cityService
                    .GetAllCitiesByCountryId<CityListModel>(user.CountryId)
                    .ToListAsync(),
                },
                MyAuctions = await this.auctionService
                .GetAuctionsByOwnerId<MyAuctionsViewModel>(user.Id).OrderBy(x => x.Status)
                .ToListAsync(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(MyProfileViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.userService.UpdateProfile(model, this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            return this.RedirectToAction(nameof(this.Dashboard));
        }
    }
}
