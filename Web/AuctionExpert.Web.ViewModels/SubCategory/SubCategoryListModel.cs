namespace AuctionExpert.Web.ViewModels.SubCategory
{
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class SubCategoryListModel : IMapTo<SubCategory>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<SubCategory, SubCategoryListModel>();
        }
    }
}
