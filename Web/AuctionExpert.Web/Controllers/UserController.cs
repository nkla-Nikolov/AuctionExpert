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
    using AuctionExpert.Services.Data.Review;
    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.City;
    using AuctionExpert.Web.ViewModels.Profile;
    using AuctionExpert.Web.ViewModels.Review;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class UserController : BaseController
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IAuctionService auctionService;
        private readonly IUserService userService;
        private readonly ICityService cityService;
        private readonly IReviewService reviewService;

        public UserController(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IAuctionService auctionService,
            IUserService userService,
            ICityService cityService,
            IReviewService reviewService)
        {
            this.userRepository = userRepository;
            this.auctionService = auctionService;
            this.userService = userService;
            this.cityService = cityService;
            this.reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> MyAuctions(int id = 1)
        {
            int itemsPerPage = 5;

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var myAuctions = await this.auctionService
                .GetAuctionsByOwnerId<MyAuctionsViewModel>(id, userId, itemsPerPage)
                .OrderBy(x => x.Status)
                .ToListAsync();

            var model = new MyAuctionsPaginatedViewModel()
            {
                ItemsCount = this.auctionService.MyAuctionsWithDeletedCount(userId),
                ActiveAuctionsCount = this.auctionService.MyActiveAuctionsCount(userId),
                PageNumber = id,
                ItemsPerPage = itemsPerPage,
                Auctions = myAuctions,
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await this.GetMyModel(userId);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(MyProfileViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await this.userService.UpdateProfile(model, userId);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                var viewModel = await this.GetMyModel(userId);
                return this.View(viewModel);
            }

            return this.RedirectToAction(nameof(this.MyProfile));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SellerProfile(string userId)
        {
            var user = await this.userRepository
                .AllAsNoTracking()
                .Include(x => x.City)
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            var comments = await this.reviewService
                .GetAllReviewsOnUser<ReviewViewModel>(userId)
                .ToListAsync();

            var model = new SellerProfileViewModel()
            {
                Id = user.Id,
                Address = user.CityId == null ? $"{user.Country.Name}" : $"{user.City.Name}, {user.Country.Name}",
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                PhoneNumber = user.PhoneNumber ?? "No Phone Number",
                AvatarUrl = user.ProfileImageUrl,
                Comments = comments,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SellerProfile(SellerProfileViewModel sellerProfile, string userId)
        {
            var reviewerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await this.reviewService.CommentOnUser(userId, reviewerId, sellerProfile.Comment);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            return this.RedirectToAction(nameof(this.SellerProfile), new { userId });
        }

        private async Task<MyProfileViewModel> GetMyModel(string userId)
        {
            var user = await this.userRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .Include(x => x.City)
                .FirstOrDefaultAsync();

            var cities = await this.cityService
                    .GetAllCitiesByCountryId<CityListModel>(user.CountryId)
                    .OrderBy(x => x.Name)
                    .ToListAsync();

            var model = new MyProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                CityName = user.City?.Name,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber ?? "No Phone added",
                ProfileImageUrl = user.ProfileImageUrl,
                Email = user.Email,
                UpdateProfileInput = new UpdateProfileViewModel()
                {
                    Cities = cities,
                },
            };

            return model;
        }
    }
}
