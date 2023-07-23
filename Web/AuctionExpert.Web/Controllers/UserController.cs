namespace AuctionExpert.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AuctionExpert.Factories.Auction;
    using AuctionExpert.Factories.User;
    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Services.Data.UserReview;
    using AuctionExpert.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IUserReviewService reviewService;
        private readonly IUserModelFactory userModelFactory;
        private readonly IAuctionModelFactory auctionModelFactory;

        public UserController(
            IUserService userService,
            IUserReviewService reviewService,
            IUserModelFactory userModelFactory,
            IAuctionModelFactory auctionModelFactory)
        {
            this.userService = userService;
            this.reviewService = reviewService;
            this.userModelFactory = userModelFactory;
            this.auctionModelFactory = auctionModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> MyAuctions(int id = 1)
        {
            int itemsPerPage = 5;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await this.auctionModelFactory.PrepareMyAuctionsPaginatedViewModelAsync(id, userId, itemsPerPage);
            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await this.userModelFactory.PrepareMyProfileViewModelAsync(userId);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(MyProfileViewModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!this.ModelState.IsValid)
            {
                model = await this.userModelFactory.PrepareMyProfileViewModelAsync(userId);
                return this.View(model);
            }

            try
            {
                await this.userService.UpdateProfile(model, userId);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                model = await this.userModelFactory.PrepareMyProfileViewModelAsync(userId);
                return this.View(model);
            }

            return this.RedirectToAction(nameof(this.MyProfile));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SellerProfile(string id)
        {
            try
            {
                var model = await this.userModelFactory.PrepareSellerProfileViewModelAsync(id);
                return this.View(model);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SellerProfile(SellerProfileViewModel sellerProfile, string userId)
        {
            var authorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await this.reviewService.AddCommentOnUser(sellerProfile.Comment, userId, authorId);
            }
            catch (ArgumentNullException)
            {
                this.Response.StatusCode = 404;
                return this.View("NotFound404");
            }

            return this.RedirectToAction(nameof(this.SellerProfile), new { userId });
        }
    }
}
