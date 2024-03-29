﻿namespace AuctionExpert.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Enumerations;
    using AuctionExpert.Data.Common.Models;

    using static AuctionExpert.Common.AuctionConstraintsAndMessages;

    public class Auction : BaseDeletableModel<int>
    {
        public Auction()
        {
            this.Bids = new HashSet<Bid>();
            this.Images = new HashSet<Image>();
            this.AuctionReviews = new HashSet<AuctionReview>();
            this.UsersLiked = new HashSet<ApplicationUser>();
        }

        [Required]
        [StringLength(TitleMaxLenght, MinimumLength = TitleMinLenght)]
        public string Title { get; set; }

        public int Views { get; set; }

        [Required]
        public decimal StartPrice { get; set; }

        [Required]
        public TypeSale AuctionType { get; set; }

        [Required]
        public ConditionType Condition { get; set; }

        [Required]
        public int StepAmount { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public DateTime ClosesIn { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<AuctionReview> AuctionReviews { get; set; }

        public virtual ICollection<ApplicationUser> UsersLiked { get; set; }
    }
}
