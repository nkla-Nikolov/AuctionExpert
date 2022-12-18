namespace AuctionExpert.Web.Controllers
{
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Auction;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuction : ControllerBase
    {
        private readonly IAuctionService auctionService;
        private readonly IDeletableEntityRepository<Auction> auctionRepo;

        public ApiAuction(IAuctionService auctionService, IDeletableEntityRepository<Auction> auctionRepo)
        {
            this.auctionService = auctionService;
            this.auctionRepo = auctionRepo;
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await this.auctionService.DeleteAsync(id);
            await this.auctionRepo.SaveChangesAsync();
        }
    }
}
