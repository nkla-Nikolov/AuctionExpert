namespace AuctionExpert.Web.ViewModels.Auction
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Enumerations;
    using AuctionExpert.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Http;

    using static AuctionExpert.Common.GlobalConstants.AuctionConstraintsAndMessages;

    public class AddAuctionViewModel
    {
        public AddAuctionViewModel()
        {
            this.Categories = new HashSet<CategoryListModel>();
        }

        [Required]
        [StringLength(TitleMaxLenght, ErrorMessage = LengthMessage, MinimumLength = TitleMinLenght)]
        public string Title { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, ErrorMessage = LengthMessage, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryListModel> Categories { get; set; }

        [Required]
        public int SubCateogoryId { get; set; }

        [Required]
        public ConditionType Condition { get; set; }

        [Required]
        public TypeSale Type { get; set; }

        [Required]
        [Range(1, 1000)]
        public int StepAmount { get; set; }

        [Required]
        public int StartPrice { get; set; }

        [Required]
        [Range(1, 7)]
        public int Duration { get; set; }

        [Required]
        public IFormFileCollection Images { get; set; }
    }
}
