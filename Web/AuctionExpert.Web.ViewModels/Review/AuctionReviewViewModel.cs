namespace AuctionExpert.Web.ViewModels.Review
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    using static AuctionExpert.Common.ReviewMessagesAndConstraints;

    public class AuctionReviewViewModel : IMapFrom<AuctionReview>, IHaveCustomMappings
    {
        public string AuthorId { get; set; }

        public DateTime DatePlaced { get; set; }

        [StringLength(AuctionCommentMaxLength, ErrorMessage = CommentLengthConstraint, MinimumLength = AuctionCommentMinLength)]
        public string Comment { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public string AuthorProfileImage { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AuctionReview, AuctionReviewViewModel>()
                .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(x => x.Author.FirstName))
                .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(x => x.Author.LastName))
                .ForMember(dest => dest.AuthorProfileImage, opt => opt.MapFrom(x => x.Author.ProfileImageUrl));
        }
    }
}
