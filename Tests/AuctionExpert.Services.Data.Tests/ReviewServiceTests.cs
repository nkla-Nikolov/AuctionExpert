namespace AuctionExpert.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using AuctionExpert.Data;
    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Data.Repositories;
    using AuctionExpert.Services.Data.Category;
    using AuctionExpert.Services.Data.Review;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Review;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ReviewServiceTests
    {
        private IDeletableEntityRepository<Review> reviewRepository;
        private IDeletableEntityRepository<Auction> auctionRepository;
        private IDeletableEntityRepository<ApplicationUser> userRepository;
        private ApplicationDbContext context;
        private IReviewService reviewService;

        public ReviewServiceTests()
        {
            this.SetupMapper();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CountryDb")
                .Options;

            this.context = new ApplicationDbContext(options);
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllOnAuctionShouldReturnCorrectNumber()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Description = "Some Description",
                OwnerId = "SomeGuid()",
                Title = "Some title",
                Reviews = new List<Review>
                {
                    new Review() { Comment = "Some comment" },
                    new Review() { Comment = "Some comment" },
                    new Review() { Comment = "Some comment" },
                },
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Equal(3, await this.reviewService.GetAllReviewsOnAuction<ReviewViewModel>(1).CountAsync());
        }

        [Fact]
        public async Task GetAllOnAuctionShouldReturnCorrectCommentContent()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Description = "Some Description",
                OwnerId = "SomeGuid()",
                Title = "Some title",
                Reviews = new List<Review>
                {
                    new Review() { Comment = "Some comment1" },
                    new Review() { Comment = "Some comment2" },
                    new Review() { Comment = "Some comment3" },
                },
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Equal("Some comment1", this.reviewService.GetAllReviewsOnAuction<ReviewViewModel>(1).First().Comment);
            Assert.Equal("Some comment3", this.reviewService.GetAllReviewsOnAuction<ReviewViewModel>(1).Last().Comment);
        }

        [Fact]
        public async Task GetAllOnAuctionShouldReturnZero()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Description = "Some Description",
                OwnerId = "SomeGuid()",
                Title = "Some title",
            });
            await this.auctionRepository.SaveChangesAsync();

            Assert.Equal(0, await this.reviewService.GetAllReviewsOnAuction<ReviewViewModel>(1).CountAsync());
        }

        [Fact]
        public async Task GetAllOnUserShouldReturnCorrectNumber()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.userRepository.AddAsync(new ApplicationUser()
            {
                Id = "SomeGuid",
                FirstName = "Nikola",
                LastName = "Nikolov",
                Age = 26,
                Reviews = new List<Review>
                {
                    new Review() { Comment = "Some comment" },
                    new Review() { Comment = "Some comment" },
                    new Review() { Comment = "Some comment" },
                    new Review() { Comment = "Some comment" },
                },
            });
            await this.userRepository.SaveChangesAsync();

            Assert.Equal(4, await this.reviewService.GetAllReviewsOnUser<ReviewViewModel>("SomeGuid").CountAsync());
        }

        [Fact]
        public async Task GetAllOnUserShouldReturnZero()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.userRepository.AddAsync(new ApplicationUser()
            {
                Id = "SomeGuid",
                FirstName = "Nikola",
                LastName = "Nikolov",
                Age = 26,
            });
            await this.userRepository.SaveChangesAsync();

            Assert.Equal(0, await this.reviewService.GetAllReviewsOnUser<ReviewViewModel>("SomeGuid").CountAsync());
        }

        [Fact]
        public async Task GetAllOnUserShouldReturnCorrectComments()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.userRepository.AddAsync(new ApplicationUser()
            {
                Id = "SomeGuid",
                FirstName = "Nikola",
                LastName = "Nikolov",
                Age = 26,
                Reviews = new List<Review>
                {
                    new Review() { Comment = "Some comment1" },
                    new Review() { Comment = "Some comment2" },
                    new Review() { Comment = "Some comment3" },
                    new Review() { Comment = "Some comment4" },
                },
            });
            await this.userRepository.SaveChangesAsync();

            Assert.Equal("Some comment1", this.reviewService.GetAllReviewsOnUser<ReviewViewModel>("SomeGuid").First().Comment);
            Assert.Equal("Some comment4", this.reviewService.GetAllReviewsOnUser<ReviewViewModel>("SomeGuid").Last().Comment);
        }

        [Fact]
        public async Task PlaceCommentOnUserShouldThrowExceptionIfUserIsNull()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.userRepository.AddAsync(new ApplicationUser()
            {
                Id = "UserIdGuid",
                FirstName = "Nikola",
                LastName = "Nikolov",
                Age = 26,
            });
            await this.userRepository.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.reviewService.CommentOnUser(null, "ReviewerIdGuid", "SomeComment"));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.reviewService.CommentOnUser("sasa", "ReviewerIdGuid", "SomeComment"));
        }

        [Fact]
        public async Task PlaceCommentOnUserShouldThrowExceptionIfReviewerIsNull()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.userRepository.AddAsync(new ApplicationUser()
            {
                Id = "UserIdGuid",
                FirstName = "Nikola",
                LastName = "Nikolov",
                Age = 26,
            });
            await this.userRepository.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.reviewService.CommentOnUser("UserIdGuid", null, "SomeComment"));
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.reviewService.CommentOnUser("UserIdGuid", "asasa", "SomeComment"));
        }

        [Fact]
        public async Task PlaceCommentOnUserShouldWorkCorrectly()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.userRepository.AddAsync(new ApplicationUser()
            {
                Id = "UserIdGuid",
                FirstName = "Nikola",
                LastName = "Nikolov",
                Age = 26,
            });
            await this.userRepository.AddAsync(new ApplicationUser()
            {
                Id = "ReviewerIdGuid",
                FirstName = "Petar",
                LastName = "Petrov",
                Age = 26,
            });
            await this.userRepository.SaveChangesAsync();

            await this.reviewService.CommentOnUser("UserIdGuid", "ReviewerIdGuid", "Hello");

            Assert.Equal(1, await this.reviewService.GetAllReviewsOnUser<ReviewViewModel>("UserIdGuid").CountAsync());
            Assert.Equal("Hello", this.reviewService.GetAllReviewsOnUser<ReviewViewModel>("UserIdGuid").First().Comment);
        }

        [Fact]
        public async Task PlaceCommentOnAuctionShouldThrowExceptionIfAuctionIsNull()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Description = "Some Description",
                OwnerId = "SomeGuid()",
                Title = "Some title",
            });
            await this.userRepository.AddAsync(new ApplicationUser()
            {
                Id = "CoolId",
                FirstName = "Nikola",
                LastName = "Nikolov",
                Age = 26,
            });
            await this.auctionRepository.SaveChangesAsync();
            await this.userRepository.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.reviewService.CommentOnAuction(2, "Hello", "CoolId"));
        }

        [Fact]
        public async Task PlaceCommentOnAuctionShouldThrowExceptionIfUserIsNull()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            var auction = new Auction()
            {
                Id = 1,
                Description = "Some Description",
                OwnerId = "SomeGuid()",
                Title = "Some title",
            };
            await this.userRepository.AddAsync(new ApplicationUser()
            {
                Id = "CoolId",
                FirstName = "Nikola",
                LastName = "Nikolov",
                Age = 26,
            });
            await this.auctionRepository.AddAsync(auction);
            await this.auctionRepository.SaveChangesAsync();
            await this.userRepository.SaveChangesAsync();

            await this.reviewService.CommentOnAuction(1, "Hello", "CoolId");

            Assert.Equal(1, auction.Reviews.Count);
        }

        [Fact]
        public async Task PlaceCommentOnAuctionShouldWorkCorrectly()
        {
            this.reviewRepository = new EfDeletableEntityRepository<Review>(this.context);
            this.auctionRepository = new EfDeletableEntityRepository<Auction>(this.context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.reviewService = new ReviewService(this.auctionRepository, this.reviewRepository, this.userRepository);

            await this.auctionRepository.AddAsync(new Auction()
            {
                Id = 1,
                Description = "Some Description",
                OwnerId = "SomeGuid()",
                Title = "Some title",
            });
            await this.auctionRepository.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.reviewService.CommentOnAuction(1, "Hello", "CoolId"));
        }

        private void SetupMapper()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load("AuctionExpert.Web.ViewModels"));
        }
    }
}
