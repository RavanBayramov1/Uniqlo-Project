using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_1.DataAccess;
using Uniqlo_1.Helpers;
using Uniqlo_1.ViewModels.Baskets;

namespace Uniqlo_1.ViewComponents
{
    public class LayoutHeaderViewComponent(UniqloDbContext _context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basket = BasketHelper.GetBasket(HttpContext.Request);
            var basketItems = await _context.Products.Where(x => basket.Select(y => y.Id).Contains(x.Id)).Select(x => new BasketItemVM
            {
                Id = x.Id,
                Name = x.Name,
                SellPrice = x.SellPrice,
                ImageUrl = x.CoverImage,
                Discount = x.Discount
            }).ToListAsync();
            foreach (var item in basketItems)
            {
                item.Count = basket.First(x => x.Id == item.Id).Count;
            }
            return View(basketItems);
        }
    }
}
