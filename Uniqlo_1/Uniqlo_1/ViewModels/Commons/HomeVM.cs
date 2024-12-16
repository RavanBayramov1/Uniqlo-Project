using Uniqlo_1.Models;
using Uniqlo_1.ViewModels.Products;
using Uniqlo_1.ViewModels.Sliders;

namespace Uniqlo_1.ViewModels.Commons
{
    public class HomeVM
    {
        public IEnumerable<SliderListItemVM> Sliders { get; set; }
        public IEnumerable<Brand> Brands { get; set; }

        public IEnumerable<ProductListItemVM> PopularProducts { get; set; }
    }
}
