using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Uniqlo_1.DataAccess;
using Uniqlo_1.ViewModels.Commons;
using Uniqlo_1.ViewModels.Products;
using Uniqlo_1.ViewModels.Sliders;

namespace Uniqlo_1.Controllers
{
    public class HomeController(UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new();
            vm.Sliders = await _context.Sliders.Where(x=> !x.IsDeleted).Select(x => new SliderListItemVM
            {
                ImageUrl = x.ImageUrl,
                Link = x.Link!,
                Subtitle = x.Subtitle,
                Title = x.Title
            }).ToListAsync();
            vm.Products = await _context.Products.Where(x => !x.IsDeleted).Select(x => new ProductListItemVM
            {
                CoverImage = x.CoverImage,
                Discount = x.Discount,
                Id = x.Id,
                IsInStock = x.Quantity>0,
                Name = x.Name,
                SellPrice = x.SellPrice
            }).ToListAsync();
            return View(vm);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
