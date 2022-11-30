namespace AuctionExpert.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Auction;
    using AuctionExpert.Services.Data.City;
    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.City;
    using AuctionExpert.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class UserController : BaseController
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IAuctionService auctionService;
        private readonly IUserService userService;
        private readonly ICityService cityService;

        public UserController(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IAuctionService auctionService,
            IUserService userService,
            ICityService cityService)
        {
            this.userRepository = userRepository;
            this.auctionService = auctionService;
            this.userService = userService;
            this.cityService = cityService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.userRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .Include(x => x.City)
                .FirstOrDefaultAsync();

            var bids = await this.auctionService.GetAllAuctions<Auction>().ToListAsync();
            var bidsCount = bids
                .SelectMany(x => x.Bids)
                .Where(x => x.BidderId == userId)
                .Count();

            var model = new MyProfileViewModel()
            {
                TotalBidsCount = bidsCount,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CityName = user.City?.Name,
                Username = user.UserName,
                Email = user.Email,
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
        public async Task<IActionResult> Dashboard(MyProfileViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.userService.UpdateProfile(model, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (InvalidOperationException)
            {
                //TODO: display error
            }

            return this.RedirectToAction(nameof(this.Dashboard));
        }
    }
}
