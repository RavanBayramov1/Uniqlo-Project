using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Uniqlo_1.Constants;
using Uniqlo_1.DataAccess;
using Uniqlo_1.Extensions;
using Uniqlo_1.Models;
using Uniqlo_1.ViewModels.Products;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Uniqlo_1.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
	{
		public async Task<IActionResult> Index()
		{
			return View(await _context.Products.Include(x => x.Brand).ToListAsync());
		}

		public async Task<IActionResult> Create()
		{
			ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProductCreateVM vm)
		{
			if (vm.File != null)
			{
				if (!vm.File.IsValidType("image"))
					ModelState.AddModelError("File", "File must be an image");
				if (!vm.File.IsValidSize(400))
					ModelState.AddModelError("File", "File must be less than 400kb");
			}
			if (vm.OtherFiles != null && vm.OtherFiles.Any())
			{
				if (!vm.OtherFiles.All(x => x.IsValidType("image")))
				{
					string fileNames = string.Join(',', vm.OtherFiles.Where(x => !x.IsValidType("image")).Select(x => x.FileName));
					ModelState.AddModelError("OtherFiles", fileNames + " is (are) not an image");
				}
				if (!vm.OtherFiles.All(x => x.IsValidSize(400)))
				{
					string fileNames = string.Join(',', vm.OtherFiles.Where(x => !x.IsValidSize(400)).Select(x => x.FileName));
					ModelState.AddModelError("OtherFiles", fileNames + " is (are) bigger than 400kb");
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
				ModelState.AddModelError("BrandId", "Brand not found");
				return View();
			}
			Product product = vm;
			product.CoverImage = await vm.File!.UploadAsync(_env.WebRootPath, "imgs", "products");
			product.Images = vm.OtherFiles?.Select(x => new ProductImage
			{
				ImageUrl = x.UploadAsync(_env.WebRootPath, "imgs", "products").Result
			}).ToList();
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Update(int? id)
		{
			if (id is null) return BadRequest();
			var data = await _context.Products
				.Where(x => x.Id == id)
				.Select(x => new ProductUpdateVM
				{
					Id = x.Id,
					BrandId = x.BrandId ?? 0,
					CostPrice = x.CostPrice,
					Description = x.Description,
					Discount = x.Discount,
					ImageUrl = x.CoverImage,
					Name = x.Name,
					Quantity = x.Quantity,
					SellPrice = x.SellPrice,
					OtherFilesUrls = x.Images.Select(y => y.ImageUrl)
				}).FirstOrDefaultAsync();
			if (data is null) return NotFound();
			ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted)
				.ToListAsync();
			return View(data);
		}

		[HttpPost]
		public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
		{
			if (vm.File != null)
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
			if (vm.OtherFiles?.Any() ?? false)
			{
				if (!vm.OtherFiles.All(x => x.IsValidType("image")))
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
			var update = await _context.Products.FindAsync(id);
			update.BrandId = vm.BrandId;	
			update.Name = vm.Name;
			update.Description = vm.Description;
			update.Quantity = vm.Quantity;
			update.Discount = vm.Discount;
			update.CostPrice = vm.CostPrice;
			update.SellPrice = vm.SellPrice;
			var data = await _context.Products.Include(x => x.Images)
			   .Where(x => x.Id == id)
			   .FirstOrDefaultAsync();
			data.Images.AddRange(vm.OtherFiles.Select(x => new ProductImage
			{
				ImageUrl = x.UploadAsync(_env.WebRootPath, "imgs", "products").Result
			}).ToList());
            await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
    
					

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return BadRequest();
			var data = _context.Products.Where(x => x.Id == id).FirstOrDefault();
			string imagePath = Path.Combine(_env.WebRootPath, "imgs", "products", data.CoverImage);
			if (System.IO.File.Exists(imagePath))
			{
				System.IO.File.Delete(imagePath);
			}
		    if(await _context.Products.AnyAsync(x => x.Id == id))
			{
				 _context.Products.Remove(data);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> DeleteImgs(int id, IEnumerable<string> imgNames)
		{
			int result = await _context.ProductImages.Where(x => imgNames.Contains(x.ImageUrl)).ExecuteDeleteAsync();
			if (result > 0)
			{
				var stringPath = imgNames.Select(imgs => Path.Combine(_env.WebRootPath, "imgs", "products")).ToList();
				foreach (var img in stringPath)
				{
					if(System.IO.File.Exists(img))
					{
						System.IO.File.Delete(img);
					}
				}
			}
			return RedirectToAction(nameof(Update), new { id });

		}
        public async Task<IActionResult> Hide(int id)
        {
            _context.Products
            .Where(x => x.Id == id)
            .FirstOrDefault().IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Show(int id)
        {
            _context.Products
            .Where(x => x.Id == id)
            .FirstOrDefault().IsDeleted = false;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
