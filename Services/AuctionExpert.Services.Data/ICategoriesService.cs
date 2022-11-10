namespace AuctionExpert.Services.Data
{
    using System.Linq;

    public interface ICategoriesService
    {
        IQueryable<T> GetAllCategories<T>();
    }
}
