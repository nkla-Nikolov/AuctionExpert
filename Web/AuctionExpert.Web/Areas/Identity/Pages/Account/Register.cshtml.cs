// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace AuctionExpert.Web.Areas.Identity.Pages.Account
{
    using System;
#nullable disable

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Data.Country;
    using AuctionExpert.Web.ViewModels.Country;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    using static AuctionExpert.Common.GlobalConstants.RegisterConstraintsAndMessages;

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly ICountryService countryService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            ICountryService countryService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.userRepository = userRepository;
            this.countryService = countryService;
            this.Countries = this.countryService.GetAllCountries<CountryListModel>();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public IEnumerable<CountryListModel> Countries { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(FirstNameMaxLenth, ErrorMessage = RangeMessage, MinimumLength = FirstNameMinLength)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(LastNameMaxLenth, ErrorMessage = RangeMessage, MinimumLength = LastNameMinLenth)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(UsernameMaxLength, ErrorMessage = RangeMessage, MinimumLength = UsernameMinLength)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(PasswordMaxLength, ErrorMessage = RangeMessage, MinimumLength = PasswordMinLength)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [Compare(nameof(Password), ErrorMessage = PasswordDoesNotMatchMessage)]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Range(AgeMinLength, AgeMaxLength)]
            public int Age { get; set; }

            [Required]
            public int CountryId { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/");

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                bool emailExist = this.userRepository
                    .AllAsNoTracking()
                    .Any(x => x.NormalizedEmail == this.Input.Email.ToUpper());

                if (emailExist)
                {
                    this.ModelState.AddModelError(string.Empty, EmailTaken);
                }
                else
                {
                    var user = new ApplicationUser()
                    {
                        FirstName = this.Input.FirstName,
                        LastName = this.Input.LastName,
                        UserName = this.Input.Username,
                        CountryId = this.Input.CountryId,
                        Email = this.Input.Email,
                        Age = this.Input.Age,
                    };

                    var result = await this.userManager.CreateAsync(user, this.Input.Password);

                    if (result.Succeeded)
                    {
                        this.logger.LogInformation("User created a new account with password.");

                        var userId = await this.userManager.GetUserIdAsync(user);
                        var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = this.Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: this.Request.Scheme);

                        await this.emailSender.SendEmailAsync(this.Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await this.signInManager.SignInAsync(user, isPersistent: false);
                            return this.LocalRedirect(returnUrl);
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
