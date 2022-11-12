namespace AuctionExpert.Services.Data
{
    using System.Linq;

    public interface ICategoryService
    {
        IQueryable<T> GetAllCategories<T>();
    }
}
