using AuctionExpert.Data.Common.Repositories;
using AuctionExpert.Data.Models;
using AuctionExpert.Services.Data.City;
using AuctionExpert.Services.Data.UserReview;
using AuctionExpert.Web.ViewModels.City;
using AuctionExpert.Web.ViewModels.Profile;
using AuctionExpert.Web.ViewModels.Review;
using Microsoft.EntityFrameworkCore;

namespace AuctionExpert.Factories.User
{
    public class UserModelFactory : IUserModelFactory
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly ICityService cityService;
        private readonly IUserReviewService userReviewService;

        public UserModelFactory(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            ICityService cityService,
            IUserReviewService userReviewService)
        {
            this.userRepository = userRepository;
            this.cityService = cityService;
            this.userReviewService = userReviewService;
        }

        public async Task<MyProfileViewModel> PrepareMyProfileViewModelAsync(string userId)
        {
            var user = await this.userRepository
                .AllAsNoTracking()
                .Where(x => x.Id == userId)
                .Include(x => x.City)
                .FirstOrDefaultAsync();

            var model = new MyProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                CityName = user.City?.Name,
                CityId = user.CityId,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber ?? "No Phone added",
                ProfileImageUrl = user.ProfileImageUrl,
                Email = user.Email,
                UpdateProfileInput = await this.PrepareUpdateProfileViewModelAsync(user.CountryId),
            };

            return model;
        }

        public async Task<SellerProfileViewModel> PrepareSellerProfileViewModelAsync(string userId)
        {
            var user = await this.userRepository
                .AllAsNoTracking()
                .Include(x => x.City)
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("User does not exist");
            }

            var comments = await this.userReviewService
                .GetAllUserReviewsByUserIdAsync<UserReviewViewModel>(userId)
                .ToListAsync();

            var model = new SellerProfileViewModel()
            {
                Id = user.Id,
                Address = user.CityId == null ? $"{user.Country.Name}" : $"{user.City.Name}, {user.Country.Name}",
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                PhoneNumber = user.PhoneNumber ?? "No Phone Number",
                AvatarUrl = user.ProfileImageUrl,
                Comments = comments,
            };

            return model;
        }

        public async Task<UpdateProfileViewModel> PrepareUpdateProfileViewModelAsync(int countryId)
        {
            var cities = await this.cityService
                    .GetAllCitiesByCountryId<CityListModel>(countryId)
                    .OrderBy(x => x.Name)
                    .ToListAsync();

            var model = new UpdateProfileViewModel
            {
                Cities = cities,
            };

            return model;
        }
    }
}
