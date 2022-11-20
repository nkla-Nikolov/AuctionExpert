namespace AuctionExpert.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data;
    using AuctionExpert.Web.ViewModels;
    using AuctionExpert.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class HomeController : BaseController
    {
        private readonly IAuctionService auctionService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IAuctionService auctionService,
            UserManager<ApplicationUser> userManager)
        {
            this.auctionService = auctionService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var auctions = await this.auctionService.GetAllAuctionsAsHomeModel().ToListAsync();

            return this.View(auctions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
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
                .GetAuctionsByOwnerId(user.Id)
                .ToListAsync(),
            };

            return this.View(model);
        }
    }
}
