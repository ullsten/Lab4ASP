using Lab4ASP.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab4ASP.Controllers
{
    public class AdminController : Controller
    {

        // GET: AdminController
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
