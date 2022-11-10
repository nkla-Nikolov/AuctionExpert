namespace AuctionExpert.Services.Data
{
    using System.Linq;

    public interface ISubCategoriesService
    {
        IQueryable<T> GetAllSubCategories<T>();

        IQueryable<T> GetAllByCategoryId<T>(int categoryId);
    }
}
