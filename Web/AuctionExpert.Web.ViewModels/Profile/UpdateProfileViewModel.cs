namespace AuctionExpert.Web.ViewModels.Profile
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Web.ViewModels.City;

    using static AuctionExpert.Common.GlobalConstants.RegisterConstraintsAndMessages;

    public class UpdateProfileViewModel
    {
        public UpdateProfileViewModel()
        {
            this.Cities = new HashSet<CityListModel>();
        }

        [StringLength(FirstNameMaxLenth, ErrorMessage = RangeMessage, MinimumLength = FirstNameMinLength)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(LastNameMaxLenth, ErrorMessage = RangeMessage, MinimumLength = LastNameMinLenth)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(PasswordMaxLength, ErrorMessage = RangeMessage, MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = PasswordDoesNotMatchMessage)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<CityListModel> Cities { get; set; }

        public int? CityId { get; set; }
    }
}
