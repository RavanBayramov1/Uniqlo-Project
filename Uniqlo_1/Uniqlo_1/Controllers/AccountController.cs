using Microsoft.AspNetCore.Mvc;

namespace Uniqlo_1.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
