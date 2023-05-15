using Microsoft.AspNetCore.Mvc;

namespace Lab4ASP.Controllers
{
    public class LoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
