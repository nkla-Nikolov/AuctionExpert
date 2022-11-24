namespace AuctionExpert.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data;
    using AuctionExpert.Web.ViewModels.Auction;
    using AuctionExpert.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class UserController : BaseController
    {
        private readonly IAuctionService auctionService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(
            IAuctionService auctionService,
            UserManager<ApplicationUser> userManager)
        {
            this.auctionService = auctionService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var user = await this.userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new MyProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                MyAuctions = await this.auctionService
                .GetAuctionsByOwnerId<MyAuctionsViewModel>(user.Id).OrderBy(x => x.Status)
                .ToListAsync(),
            };

            return this.View(model);
        }
    }
}
