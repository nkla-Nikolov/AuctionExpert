using Microsoft.AspNetCore.Mvc;

namespace AuctionExpert.Web.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
