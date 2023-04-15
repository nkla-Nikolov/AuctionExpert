namespace AuctionExpert.Web.ViewModels.Blog
{
    using System;
    using System.Collections.Generic;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AuctionExpert.Web.ViewModels.Review;
    using AutoMapper;

    public class BlogPostViewModel : IMapFrom<BlogPost>, IHaveCustomMappings
    {
        public BlogPostViewModel()
        {
            this.Reviews = new List<BlogPostReviewViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<BlogPostReviewViewModel> Reviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<BlogPost, BlogPostViewModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(x => x.Image.UrlPath));
        }
    }
}
