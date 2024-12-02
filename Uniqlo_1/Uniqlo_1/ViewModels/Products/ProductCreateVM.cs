using System.ComponentModel.DataAnnotations;
using Uniqlo_1.Models;

namespace Uniqlo_1.ViewModels.Products
{
    public class ProductCreateVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        [Range(0,100)]
        public int Discount { get; set; }
        public int BrandId { get; set; }
        public IFormFile File { get; set; }
        public ICollection<IFormFile> OtherFiles { get; set; }
        public static implicit operator Product(ProductCreateVM vm)
        {
            return new Product
            {
                Name = vm.Name,
                Description = vm.Description,
                Quantity = vm.Quantity,
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                Discount = vm.Discount, 
                BrandId = vm.BrandId  
            };
        }
    }
}
