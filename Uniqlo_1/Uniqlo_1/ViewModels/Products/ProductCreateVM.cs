using System.ComponentModel.DataAnnotations;
using Uniqlo_1.Models;

namespace Uniqlo_1.ViewModels.Products
{
    public class ProductCreateVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Discount { get; set; }
        public int BrandId { get; set; }
        public IFormFile File { get; set; }
    }
}
