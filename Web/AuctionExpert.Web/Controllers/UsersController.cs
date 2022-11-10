namespace AuctionExpert.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        public IActionResult Dashboard()
        {
            return this.View();
        }
    }
}
