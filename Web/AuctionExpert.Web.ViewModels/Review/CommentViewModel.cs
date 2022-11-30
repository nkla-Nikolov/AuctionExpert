namespace AuctionExpert.Web.ViewModels.Review
{
    using System;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class CommentViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public DateTime DatePlaced { get; set; }

        public string Content { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, CommentViewModel>()
                .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(x => x.User.FirstName))
                .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(x => x.User.LastName))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(x => x.Comment));
        }
    }
}
