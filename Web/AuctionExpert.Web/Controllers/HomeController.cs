namespace AuctionExpert.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using AuctionExpert.Services.Data;
    using AuctionExpert.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
            var auctions = await this.auctionService.GetAllAuctionsAsHomeModel();

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
