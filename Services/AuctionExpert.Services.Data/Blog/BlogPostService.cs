namespace AuctionExpert.Services.Data.Blog
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class BlogPostService : IBlogPostService
    {
        private readonly IDeletableEntityRepository<BlogPost> blogPostRepository;

        public BlogPostService(IDeletableEntityRepository<BlogPost> blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        public Task CreateBlogAsync()
        {
            throw new NotImplementedException();
        }

        public Task EditBlogAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAllBlogPostsAsync<T>()
        {
            return this.blogPostRepository
                .AllAsNoTracking()
                .To<T>();
        }
    }
}
