using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_1.DataAccess;
using Uniqlo_1.Models;
using Uniqlo_1.ViewModels.Brands;

namespace Uniqlo_1.Areas.Admin.Controllers;

[Area("Admin")]
public class BrandController(UniqloDbContext _context):Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await _context.Brands.ToListAsync());
    }
    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(BrandCreateVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        Brand brand = new Brand
        {
            Name = vm.Name
        };
        await _context.Brands.AddAsync(brand);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var data = await _context.Brands.Where(x=> x.Id == id).FirstOrDefaultAsync();
        if(data is null) return NotFound();
        _context.Brands.Remove(data);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
