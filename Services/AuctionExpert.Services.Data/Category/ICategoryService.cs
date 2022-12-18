namespace AuctionExpert.Services.Data.Category
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        IQueryable<T> GetAllCategories<T>();

        Task<bool> Exist(int categoryId);
    }
}
