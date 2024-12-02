using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Uniqlo_1.Constants;
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
            return View(await _context.Products.Include(x=> x.Brand).ToListAsync());
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
                if (!vm.File.IsValidType("image"))
                {
                    ModelState.AddModelError("File", "File must be an image!");
                }
                if (!vm.File.IsValidSize(400))
                {
                    ModelState.AddModelError("File", "File size must be less than 400kb!");
                }
            }
            if (vm.OtherFiles.Any()) 
            {
                if(!vm.OtherFiles.All(x=> x.IsValidType("image")))
                {
                    string fileNames = string.Join(',', vm.OtherFiles.Where(x => !x.IsValidType(ContentTypes.Image)).Select(x => x.FileName));
                    ModelState.AddModelError("OtherFiles", fileNames + "is(are) not an image!");
                }
                if (!vm.OtherFiles.All(x => x.IsValidSize(400)))
                {
                    string fileNames = string.Join(',', vm.OtherFiles.Where(x => !x.IsValidSize(ContentTypes.StaticFileSize)).Select(x => x.FileName));
                    ModelState.AddModelError("OtherFiles", fileNames + "is(are) bigger than 400kb!");
                }
            }   
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
                return View(vm);
            }
            if (!await _context.Brands.AnyAsync(x => x.Id == vm.BrandId))
            {
                ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
                ModelState.AddModelError("BrandId", "Brand not found!");
                return View();
            }
            Product product = vm;
            product.CoverImage = await vm.File!.UploadAsync(_env.WebRootPath, "imgs", "products");
            product.Images = vm.OtherFiles.Select(x => new ProductImage
            {
                ImageUrl = x.UploadAsync(_env.WebRootPath, "imgs", "products").Result 
            }).ToList();
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //public async Task<IActionResult> Update(int? id)
        //{
        //    if (id is null) return BadRequest();
        //    var data = await _context.Products.Where(x => x.Id == id).Select(x => new ProductUpdateVM
        //    {
        //        Name = x.Name,
        //        Description = x.Description,
        //        Quantity = x.Quantity,
        //        CostPrice = x.CostPrice,
        //        SellPrice = x.SellPrice,
        //        Discount = x.Discount
        //    });

        //}
    }
}
