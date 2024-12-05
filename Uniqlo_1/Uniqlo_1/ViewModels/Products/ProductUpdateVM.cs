using System.ComponentModel.DataAnnotations;
using Uniqlo_1.Models;

namespace Uniqlo_1.ViewModels.Products
{
    public class ProductUpdateVM
    {
		public int Id { get; set; }	
		public string Name { get; set; }
		public string Description { get; set; }
		[Range(0, int.MaxValue)]
		public int Quantity { get; set; }
		public decimal CostPrice { get; set; }
		public decimal SellPrice { get; set; }
		[Range(0, 100)]
		public int Discount { get; set; }
		public int BrandId { get; set; }
		public IFormFile File { get; set; }
		public string? ImageUrl { get; set; }
		public IEnumerable<string>? OtherFilesUrls { get; set; }
		public ICollection<IFormFile>? OtherFiles { get; set; }
	}
}
