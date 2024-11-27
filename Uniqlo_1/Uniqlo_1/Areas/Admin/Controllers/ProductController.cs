using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_1.DataAccess;
using Uniqlo_1.ViewModels.Products;

namespace Uniqlo_1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            return View();
        }
    }
}
