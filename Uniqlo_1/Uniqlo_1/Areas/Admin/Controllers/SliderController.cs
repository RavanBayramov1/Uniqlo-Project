using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_1.DataAccess;
using Uniqlo_1.Models;
using Uniqlo_1.ViewModels.Sliders;

namespace Uniqlo_1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            if (!vm.File.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("File", "Format type must be image!");
                return View(vm);
            }
            if (vm.File.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "File size must be less than 2 mb!");
                return View(vm);
            }
            if (!Path.Exists(Path.Combine(_env.WebRootPath, "imgs", "sliders")))
            {
                Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "imgs", "sliders"));
            }
            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);
            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "sliders", newFileName)))
            {
                await vm.File.CopyToAsync(stream);
            }
            Slider slider = new Slider
            {
                ImageUrl = newFileName,
                Title = vm.Title,
                Subtitle = vm.Subtitle,
                Link = vm.Link
            };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var data = await _context.Sliders
                .Where(x => x.Id == id)
                .Select(x => new SliderUpdateVM
                {
                    Title= x.Title,
                    Subtitle= x.Subtitle,
                    Link = x.Link,
                    ImageUrl = x.ImageUrl,
                })
                .FirstOrDefaultAsync();
            if (data is null) return NotFound();
            ViewBag.Categories = await _context.Sliders.Where(x => !x.IsDeleted)
                .ToListAsync();
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM vm)
        {
            var data = await _context.Sliders
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);
            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "sliders", newFileName)))
            {
                await vm.File.CopyToAsync(stream);
            }
            data.Title = vm.Title;
            data.Subtitle = vm.Subtitle;
            data.Link = vm.Link;
            data.ImageUrl = newFileName;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id, SliderCreateVM vm)
        {
            Slider slider = await _context.Sliders.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (slider == null) ModelState.AddModelError("File", "File not found!");
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            System.IO.File.Delete(Path.Combine(_env.WebRootPath, "imgs", "sliders", vm.File.FileName));
            return RedirectToAction(nameof(Index));
        }
        public  async Task<IActionResult> Hide(int id)
        {
            _context.Sliders
            .Where(x => x.Id == id)
            .FirstOrDefault().IsDeleted = true;
            _context.SaveChanges (); 
            return RedirectToAction(nameof(Index));    
        }
        public async Task<IActionResult> Show(int id)
        {
            _context.Sliders
            .Where(x => x.Id == id)
            .FirstOrDefault().IsDeleted = false;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    } 
}
