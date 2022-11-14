namespace AuctionExpert.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Auction;
    using Microsoft.EntityFrameworkCore;

    public class AuctionService : IAuctionService
    {
        private readonly IDeletableEntityRepository<Auction> auctionRepository;
        private readonly IImageService imageService;

        public AuctionService(
            IDeletableEntityRepository<Auction> auctionRepository,
            IImageService imageService)
        {
            this.auctionRepository = auctionRepository;
            this.imageService = imageService;
        }

        public async Task CreateAsync(AddAuctionViewModel model, ApplicationUser user)
        {
            var auction = new Auction()
            {
                AuctionType = model.Type,
                CountryId = user.CountryId,
                Duration = model.Duration,
                ClosesIn = DateTime.UtcNow.AddDays(model.Duration),
                Description = model.Description,
                OwnerId = user.Id,
                StartPrice = model.StartPrice,
                SubCategoryId = model.SubCateogoryId,
                Title = model.Title,
            };

            var images = await this.imageService.UploadImages(model.Images);
            auction.Images = images.ToList();

            await this.auctionRepository.AddAsync(auction);
            await this.auctionRepository.SaveChangesAsync();
        }

        public async Task<List<HomeAuctionViewModel>> GetAllAuctions()
        {
            return await this.auctionRepository
                .AllAsNoTracking()
                .Select(x => new HomeAuctionViewModel()
                {
                    AuthorName = x.Owner.FirstName,
                    ClosesIn = x.ClosesIn,
                    LastBid = x.Bids == null ? x.Bids.OrderBy(x => x.TimePlaced).First().MoneyPlaced : x.StartPrice,
                    MainImage = x.Images.First().UrlPath,
                    Title = x.Title,
                    Views = x.Views,
                })
                .ToListAsync();
        }
    }
}
