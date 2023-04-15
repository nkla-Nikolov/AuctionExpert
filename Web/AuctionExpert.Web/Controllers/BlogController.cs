namespace AuctionExpert.Web.Controllers
{
    using System.Threading.Tasks;

    using AuctionExpert.Services.Data.Blog;
    using AuctionExpert.Web.ViewModels.Blog;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class BlogController : BaseController
    {
        private readonly IBlogPostService blogPostService;

        public BlogController(IBlogPostService blogPostService)
        {
            this.blogPostService = blogPostService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await this.blogPostService
                .GetAllBlogPostsAsync<BlogPostViewModel>()
                .ToListAsync();

            return this.View(posts);
        }
    }
}
