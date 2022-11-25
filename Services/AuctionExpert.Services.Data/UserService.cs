﻿namespace AuctionExpert.Services.Data
{
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
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

            if (model.UpdateProfileInput.Email != null)
            {
                user.Email = model.UpdateProfileInput.Email;
            }

            if (model.UpdateProfileInput.Password != null)
            {
                //TODO : Change pass
            }

            if (model.UpdateProfileInput.CityId != null)
            {
                user.Country.ci
            }

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();
        }
    }
}
