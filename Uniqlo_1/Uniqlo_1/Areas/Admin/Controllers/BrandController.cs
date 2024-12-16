using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_1.DataAccess;
using Uniqlo_1.Models;
using Uniqlo_1.ViewModels.Brands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
    public async Task<IActionResult> Update(int? id)
    {
        if (id is null) return BadRequest();
        var data = await _context.Brands.Where(x => x.Id == id).Select(x=> new BrandUpdateVM
        {
            Name=x.Name
        }).FirstOrDefaultAsync();
        if(data is null) return NotFound();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, BrandUpdateVM vm)
    {
        if(id is null) return BadRequest();
        var data = await _context.Brands.FindAsync(id);
        data.Name = vm.Name;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        //var data = await _context.Products.Where(x=> x.BrandId == id).FirstOrDefaultAsync();
        //data.BrandId = null;

        if ( await _context.Brands.AnyAsync(x=> x.Id == id))
        {
            _context.Brands.Remove(new Brand
            {
                Id = id.Value
            });
            await _context.SaveChangesAsync();
        }
        
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
