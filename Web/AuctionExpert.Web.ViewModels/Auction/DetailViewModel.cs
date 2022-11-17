namespace AuctionExpert.Web.ViewModels.Auction
{
    using System.Collections.Generic;

    using AuctionExpert.Web.ViewModels.Image;

    public class DetailViewModel
    {
        public DetailViewModel()
        {
            this.Images = new HashSet<DetailsImageViewModel>();
            this.Bidders = new List<BidderViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal BiddingPrice { get; set; }

        public int CurrentBid { get; set; }

        public IEnumerable<DetailsImageViewModel> Images { get; set; }

        public IEnumerable<BidderViewModel> Bidders { get; set; }
    }
}
