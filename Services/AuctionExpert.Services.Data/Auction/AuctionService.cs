namespace AuctionExpert.Services.Data.Auction
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Bid;
    using AuctionExpert.Services.Data.Image;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Auction;
    using Microsoft.EntityFrameworkCore;

    using static AuctionExpert.Common.AuctionConstraintsAndMessages;

    public class AuctionService : IAuctionService
    {
        private readonly IDeletableEntityRepository<Auction> auctionRepository;
        private readonly IImageService imageService;
        private readonly IBidService bidService;

        public AuctionService(
            IDeletableEntityRepository<Auction> auctionRepository,
            IImageService imageService,
            IBidService bidService)
        {
            this.auctionRepository = auctionRepository;
            this.imageService = imageService;
            this.bidService = bidService;
        }

        public IQueryable<T> GetAllAuctions<T>()
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .To<T>();
        }

        public async Task<Auction> GetAuctionByIdAsync(int auctionId)
        {
            return await this.auctionRepository
                .All()
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .FirstOrDefaultAsync(x => x.Id == auctionId);
        }

        public IQueryable<T> GetAuctionsByOwnerId<T>(int page, string ownerId, int itemsPerPage)
        {
            return this.auctionRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.OwnerId == ownerId)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>();
        }

        public IQueryable<T> GetAllAuctionsByCategoryId<T>(int page, int itemsPerPage, int categoryId)
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>();
        }

        public IQueryable<T> GetAllAuctionsByCountryId<T>(int countryId)
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.CountryId == countryId)
                .To<T>();
        }

        public IQueryable<T> GetAllPaginatedAuctions<T>(int page, int itemsPerPage)
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>();
        }

        public async Task DeleteAsync(int auctionId)
        {
            var auction = await this.GetAuctionByIdAsync(auctionId);

            if (auction == null)
            {
                throw new ArgumentNullException();
            }

            this.auctionRepository.Delete(auction);
            await this.auctionRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(AddAuctionViewModel model, ApplicationUser user)
        {
            var auction = new Auction
            {
                AuctionType = model.AuctionType,
                CountryId = user.CountryId,
                Duration = model.Duration,
                Condition = model.Condition,
                StepAmount = model.StepAmount,
                ClosesIn = DateTime.UtcNow.AddDays(model.Duration),
                Description = model.Description,
                OwnerId = user.Id,
                StartPrice = model.StartPrice,
                CategoryId = model.CategoryId,
                SubCategoryId = model.SubCateogoryId,
                Title = model.Title,
            };

            var images = await this.imageService.UploadImages(model.Images);
            auction.Images = images.ToList();

            await this.auctionRepository.AddAsync(auction);
            await this.auctionRepository.SaveChangesAsync();
        }

        public async Task PlaceBidAsync(int? currentBid, string userId, int auctionId)
        {
            var auction = await this.GetAuctionByIdAsync(auctionId);

            if (auction == null)
            {
                throw new ArgumentNullException();
            }

            var lastBid = await this.bidService.GetLastHighestBid(auction.Id);

            if (currentBid == null)
            {
                throw new InvalidOperationException(InvalidInputBidValue);
            }

            if (currentBid < auction.StartPrice + auction.StepAmount ||
                currentBid < lastBid + auction.StepAmount)
            {
                throw new InvalidOperationException(LowerInputBid);
            }

            var timeOffset = DateTime.UtcNow - auction.ClosesIn;

            if (timeOffset.Minutes < 6)
            {
                auction.ClosesIn.AddMinutes(3);
            }

            auction.Bids.Add(new Bid()
            {
                BidderId = userId,
                MoneyPlaced = (decimal)currentBid,
                TimePlaced = DateTime.UtcNow,
            });

            await this.auctionRepository.SaveChangesAsync();
        }

        public async Task EditAuction(int auctionId, EditAuctionInputModel model)
        {
            var auction = await this.GetAuctionByIdAsync(auctionId);

            if (auction == null)
            {
                throw new ArgumentNullException();
            }

            var images = await this.imageService.UploadImages(model.Images);

            auction.AuctionType = model.AuctionType;
            auction.Condition = model.Condition;
            auction.Description = model.Description;
            auction.StartPrice = model.StartPrice;
            auction.StepAmount = model.StepAmount;
            auction.CategoryId = model.CategoryId;
            auction.SubCategoryId = model.SubCateogoryId;
            auction.Title = model.Title;
            auction.Images = images.ToList();

            await this.auctionRepository.SaveChangesAsync();
        }

        public int MyAuctionsWithDeletedCount(string ownerId)
        {
            return this.auctionRepository
                .AllAsNoTrackingWithDeleted()
                .Where(x => x.OwnerId == ownerId)
                .Count();
        }

        public int MyActiveAuctionsCount(string ownerId)
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.OwnerId == ownerId)
                .Count();
        }

        public int AllAuctionsCount()
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Count();
        }

        public int AllAuctionsInCategoryCount(int categoryId)
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .Count();
        }

        public IQueryable<T> GetAllCommentsByAuctionId<T>(int auctionId)
        {
            return this.auctionRepository
                .AllAsNoTracking()
                .Where(x => x.Id == auctionId)
                .Select(x => x.AuctionReviews)
                .To<T>();
        }

        public async Task LikeAuction(Auction auction, ApplicationUser user)
        {
            auction.UsersLiked.Add(user);
            await this.auctionRepository.SaveChangesAsync();
        }
    }
}
