using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Uniqlo_1.DataAccess;
using Uniqlo_1.Extensions;
using Uniqlo_1.Models;
using Uniqlo_1.ViewModels.Products;

namespace Uniqlo_1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(UniqloDbContext _context,IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Brands.Where(x=> !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if(vm.File != null)
            {
                if (vm.File.IsValidType("image"))
                {
                    ModelState.AddModelError("File", "File must be an image!");
                }
                if (vm.File.IsValidSize(400))
                {
                    ModelState.AddModelError("File", "File size must be less than 400kb!");
                }
            }
            if (!ModelState.IsValid) return View(vm);
            if (!await _context.Brands.AnyAsync(x => x.Id == vm.BrandId))
            {
                ModelState.AddModelError("BrandId", "Brand not found!");
                return View();
            }
            Product product = vm;
            product.CoverImage = await vm.File!.UploadAsync(_env.WebRootPath, "imgs", "products");
            await _context.AddAsync(product);   
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //public async Task<IActionResult> Update(int id, Product product)
        //{

        //}
    }
}
