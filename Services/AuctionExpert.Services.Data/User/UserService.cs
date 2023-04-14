namespace AuctionExpert.Services.Data.User
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Image;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IImageService imageService;

        public UserService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IImageService imageService)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.imageService = imageService;
        }

        public IQueryable<T> GetAllUsers<T>()
        {
            return this.userRepository
                .AllAsNoTracking()
                .To<T>();
        }

        public IQueryable<T> GetAllUsersPaginated<T>(int page, int itemsPerPage = 50)
        {
            return this.userRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>();
        }

        public int GetCount()
        {
            return this.userRepository
                .AllAsNoTracking()
                .Count();
        }

        public async Task AddUserToRole(string userId, string roleName)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var roleExists = await this.roleManager.RoleExistsAsync(roleName);

            if (user == null || roleExists == false)
            {
                throw new ArgumentNullException();
            }

            await this.userManager.AddToRoleAsync(user, roleName);
        }

        public async Task RemoveUserFromRole(string userId, string roleName)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var roleExists = await this.roleManager.RoleExistsAsync(roleName);

            if (user == null || roleExists == false)
            {
                throw new ArgumentNullException();
            }

            await this.userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task UpdateProfile(MyProfileViewModel model, string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (model.UpdateProfileInput.FirstName != null)
            {
                user.FirstName = model.UpdateProfileInput.FirstName;
            }

            if (model.UpdateProfileInput.LastName != null)
            {
                user.LastName = model.UpdateProfileInput.LastName;
            }

            if (model.UpdateProfileInput.OldPassword != null && model.UpdateProfileInput.Password != null)
            {
                var result = await this.userManager.ChangePasswordAsync(user, model.UpdateProfileInput.OldPassword, model.UpdateProfileInput.Password);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Ivalid password! Please, try again.");
                }
            }

            if (model.UpdateProfileInput.CityId != null)
            {
                user.CityId = model.UpdateProfileInput.CityId;
            }

            if (model.UpdateProfileInput.Image != null)
            {
                var imageUrl = await this.imageService.UploadProfileImage(model.UpdateProfileInput.Image);
                user.ProfileImageUrl = imageUrl;
            }

            if (model.UpdateProfileInput.PhoneNumber != null)
            {
                user.PhoneNumber = model.UpdateProfileInput.PhoneNumber;
            }

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();
        }

        public Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return this.userManager.FindByIdAsync(userId);
        }
    }
}
