namespace AuctionExpert.Web.ViewModels.Authentication
{
    using System.ComponentModel.DataAnnotations;

    using static AuctionExpert.Common.GlobalConstants.RegisterConstraints;

    public class RegisterViewModel
    {
        [Required]
        [StringLength(
            UsernameMaxLength,
            ErrorMessage = "The {0} must be at least {2} and maximum {1} characters long!",
            MinimumLength = UsernameMinLength)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(
            PasswordMaxLength,
            ErrorMessage = "The {0} must be at least {2} and maximum {1} characters long!",
            MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(
            "Password",
            ErrorMessage = "The password and confirmation password do not match!")]
        public string ConfirmPassword { get; set; }
    }
}
