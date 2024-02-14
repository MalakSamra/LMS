using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
