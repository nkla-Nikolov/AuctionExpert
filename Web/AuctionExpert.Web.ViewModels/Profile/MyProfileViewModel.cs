namespace AuctionExpert.Web.ViewModels.Profile
{
    using System.Collections.Generic;

    public class MyProfileViewModel
    {
        public MyProfileViewModel()
        {
            this.MyAuctions = new HashSet<MyAuctionsViewModel>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public int ParticipatedInAuctionsCount { get; set; }

        public int AuctionsWon { get; set; }

        public IEnumerable<MyAuctionsViewModel> MyAuctions { get; set; }
    }
}
