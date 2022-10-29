// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace AuctionExpert.Web.Areas.Identity.Pages.Account
{
#nullable disable

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Web.ViewModels.Authentication;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using static AuctionExpert.Common.GlobalConstants.RegisterConstraints;

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.userRepository = userRepository;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
                bool usernameExist = await this.userRepository
                    .AllAsNoTracking()
                    .AnyAsync(x => x.NormalizedUserName == this.Input.Username.ToLower());

                bool emailExist = await this.userRepository
                    .AllAsNoTracking()
                    .AnyAsync(x => x.NormalizedEmail == this.Input.Email.ToLower());

                if (usernameExist)
                {
                    this.ModelState.AddModelError(string.Empty, UsernameTaken);
                }
                else if (emailExist)
                {
                    this.ModelState.AddModelError(string.Empty, EmailTaken);
                }
                else
                {
                    var user = new ApplicationUser()
                    {
                        UserName = this.Input.Username,
                        Email = this.Input.Email,
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
