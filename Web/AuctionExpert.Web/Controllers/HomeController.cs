namespace AuctionExpert.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using AuctionExpert.Services.Data.Auction;
    using AuctionExpert.Web.ViewModels;
    using AuctionExpert.Web.ViewModels.Auction;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class HomeController : BaseController
    {
        private readonly IAuctionService auctionService;

        public HomeController(
            IAuctionService auctionService)
        {
            this.auctionService = auctionService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var auctions = await this.auctionService.GetAllAuctions<HomeAuctionViewModel>().ToListAsync();

            return this.View(auctions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
