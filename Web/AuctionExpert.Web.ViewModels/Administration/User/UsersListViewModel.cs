namespace AuctionExpert.Web.ViewModels.Administration.User
{
    using System;
    using System.Linq;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;
    using AutoMapper;

    public class UsersListViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string ProfileImageUrl { get; set; }

        public string CreatedOn { get; set; }

        public int AuctionsCount { get; set; }

        public bool IsAdmin { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UsersListViewModel>()
            .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(x => x.Roles.Count() > 0 ? false : true))
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn.ToShortDateString()));
        }
    }
}
