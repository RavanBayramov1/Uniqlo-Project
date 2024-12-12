using Microsoft.AspNetCore.Mvc;

namespace Uniqlo_1.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
