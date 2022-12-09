namespace AuctionExpert.Web.ViewModels.Profile
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Web.ViewModels.City;
    using Microsoft.AspNetCore.Http;

    using static AuctionExpert.Common.GlobalConstants.RegisterConstraintsAndMessages;

    public class UpdateProfileViewModel
    {
        public UpdateProfileViewModel()
        {
            this.Cities = new HashSet<CityListModel>();
        }

        public string PhoneNumber { get; set; }

        public IFormFile Image { get; set; }

        [StringLength(FirstNameMaxLenth, ErrorMessage = RangeMessage, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; }

        [StringLength(LastNameMaxLenth, ErrorMessage = RangeMessage, MinimumLength = LastNameMinLenth)]
        public string LastName { get; set; }

        [StringLength(PasswordMaxLength, ErrorMessage = RangeMessage, MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [StringLength(PasswordMaxLength, ErrorMessage = RangeMessage, MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = PasswordDoesNotMatchMessage)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public IEnumerable<CityListModel> Cities { get; set; }

        public int? CityId { get; set; }
    }
}
