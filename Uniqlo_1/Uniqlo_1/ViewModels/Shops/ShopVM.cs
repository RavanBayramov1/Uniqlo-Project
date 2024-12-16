using Uniqlo_1.ViewModels.Brands;
using Uniqlo_1.ViewModels.Products;

namespace Uniqlo_1.ViewModels.Shops
{
    public class ShopVM
    {
        public IEnumerable<BrandAndProductVM> Brands { get; set; }  
        public IEnumerable<ProductListItemVM> Products { get; set; }
        public int ProductCount { get; set; }
    }
}
