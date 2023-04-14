namespace AuctionExpert.Web.ViewModels.Review
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    using static AuctionExpert.Common.ReviewMessagesAndConstraints;

    public class UserReviewViewModel : IMapFrom<UserReview>, IHaveCustomMappings
    {
        public string AuthorId { get; set; }

        public DateTime DatePlaced { get; set; }

        [StringLength(UserCommentMaxLength, ErrorMessage = CommentLengthConstraint, MinimumLength = UserCommentMinLength)]
        public string Comment { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public string AuthorProfileImage { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserReview, UserReviewViewModel>()
                .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(x => x.Author.FirstName))
                .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(x => x.Author.LastName))
                .ForMember(dest => dest.AuthorProfileImage, opt => opt.MapFrom(x => x.Author.ProfileImageUrl));
        }
    }
}
