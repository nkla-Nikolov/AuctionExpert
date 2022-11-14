namespace AuctionExpert.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Auction;

    public interface IAuctionService
    {
        Task CreateAsync(AddAuctionViewModel model, ApplicationUser user);

        Task<List<HomeAuctionViewModel>> GetAllAuctions();
    }
}
