namespace AuctionExpert.Web.ViewModels.Review
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    using static AuctionExpert.Common.ReviewMessagesAndConstraints;

    public class ReviewViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public string ReviewerId { get; set; }

        public DateTime DatePlaced { get; set; }

        [StringLength(AuctionCommentMaxLength, ErrorMessage = CommentLengthConstraint, MinimumLength = AuctionCommentMinLength)]
        public string Comment { get; set; }

        public string ReviewerFirstName { get; set; }

        public string ReviewerLastName { get; set; }

        public string ReviewerProfileImage { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewViewModel>()
                .ForMember(dest => dest.ReviewerId, opt => opt.MapFrom(x => x.ReviewerId))
                .ForMember(dest => dest.ReviewerFirstName, opt => opt.MapFrom(x => x.Reviewer.FirstName))
                .ForMember(dest => dest.ReviewerLastName, opt => opt.MapFrom(x => x.Reviewer.LastName))
                .ForMember(dest => dest.ReviewerProfileImage, opt => opt.MapFrom(x => x.Reviewer.ProfileImageUrl));
        }
    }
}
