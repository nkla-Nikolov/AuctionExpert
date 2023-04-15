namespace AuctionExpert.Services.Data.Blog
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBlogPostService
    {
        public IQueryable<T> GetAllBlogPostsAsync<T>();

        public Task CreateBlogAsync();

        public Task EditBlogAsync();
    }
}
