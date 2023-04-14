namespace AuctionExpert.Web.ViewModels.Auction
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AuctionExpert.Data.Common.Enumerations;
    using AuctionExpert.Web.ViewModels.Image;
    using AuctionExpert.Web.ViewModels.Review;

    public class DetailViewModel
    {
        public DetailViewModel()
        {
            this.Images = new HashSet<DetailsImageViewModel>();
            this.Bidders = new List<BidderViewModel>();
            this.Comments = new HashSet<AuctionReviewViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TypeSale AuctionType { get; set; }

        public decimal BiddingPrice { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public string Condition { get; set; }

        public int StepAmount { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public int? CurrentBid { get; set; }

        public string Comment { get; set; }

        public IEnumerable<DetailsImageViewModel> Images { get; set; }

        public IEnumerable<AuctionReviewViewModel> Comments { get; set; }

        public IEnumerable<BidderViewModel> Bidders { get; set; }
    }
}
