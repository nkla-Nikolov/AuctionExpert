namespace AuctionExpert.Web.Areas.Administration.Controllers
{
    using AuctionExpert.Common;
    using AuctionExpert.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
