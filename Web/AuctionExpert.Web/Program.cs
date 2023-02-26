namespace AuctionExpert.Web
{
    using System;
    using System.Linq;
    using System.Reflection;

    using AspNetCoreHero.ToastNotification;
    using AspNetCoreHero.ToastNotification.Extensions;
    using AuctionExpert.Data;
    using AuctionExpert.Data.Common;
    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Data.Repositories;
    using AuctionExpert.Data.Seeding;
    using AuctionExpert.Services.Data.Auction;
    using AuctionExpert.Services.Data.Bid;
    using AuctionExpert.Services.Data.Category;
    using AuctionExpert.Services.Data.City;
    using AuctionExpert.Services.Data.Country;
    using AuctionExpert.Services.Data.Image;
    using AuctionExpert.Services.Data.Review;
    using AuctionExpert.Services.Data.Settings;
    using AuctionExpert.Services.Data.SubCategory;
    using AuctionExpert.Services.Data.User;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Services.Messaging;
    using AuctionExpert.Web.ViewModels;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var cloudinaryName = configuration.GetValue<string>("Cloudinary:CloudName");
            var apiKey = configuration.GetValue<string>("Cloudinary:ApiKey");
            var apiSecret = configuration.GetValue<string>("Cloudinary:ApiSecret");
            var sendGridApiKey = configuration.GetValue<string>("SendGridApiKey");

            if (new[] { cloudinaryName, apiKey, apiSecret }.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Please specify Cloudinary account details!");
            }

            Account account = new Account(cloudinaryName, apiKey, apiSecret);
            Cloudinary cloudinary = new Cloudinary(account);

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });
            services.AddNotyf(config =>
            {
                config.Position = NotyfPosition.TopRight;
                config.HasRippleEffect = true;
                config.IsDismissable = true;
                config.DurationInSeconds = 5;
            });

            services.AddSingleton(configuration);
            services.AddSingleton(cloudinary);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(sendGridApiKey));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ICityService, CityService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ISubCategoryService, SubCategoryService>();
            services.AddTransient<IAuctionService, AuctionService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IBidService, BidService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReviewService, ReviewService>();
        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseNotyf();

            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
        }
    }
}
