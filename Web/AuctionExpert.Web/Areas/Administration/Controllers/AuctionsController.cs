namespace AuctionExpert.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using AuctionExpert.Services.Data.Auction;
    using AuctionExpert.Web.ViewModels.Administration.Auction;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class AuctionsController : AdministrationController
    {
        private readonly IAuctionService auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            this.auctionService = auctionService;
        }

        public async Task<IActionResult> All(int id = 1)
        {
            var itemsPerPage = 5;

            var auctions = await this.auctionService
                .GetAllPaginatedAuctions<AdminAuctionViewModel>(id, itemsPerPage)
                .ToListAsync();

            var model = new PaginatedAuctionListModel()
            {
                Auctions = auctions,
                ItemsCount = this.auctionService.AllAuctionsCount(),
                ItemsPerPage = itemsPerPage,
                PageNumber = id,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int auctionId)
        {
            return this.RedirectToAction(nameof(this.All), new { id = 1 });
        }
    }
}
