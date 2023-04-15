namespace AuctionExpert.Web.ViewModels.Review
{
    using System;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class BlogPostReviewViewModel : IMapFrom<BlogPostReview>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public DateTime DatePlaced { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<BlogPostReview, BlogPostReviewViewModel>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(x => x.UserId));
        }
    }
}
