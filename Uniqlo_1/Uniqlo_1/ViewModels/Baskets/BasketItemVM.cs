using System.ComponentModel.DataAnnotations;

namespace Uniqlo_1.ViewModels.Baskets
{
    public class BasketItemVM
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [DataType("decimal(18,2)")]
        public decimal SellPrice { get; set; }
        [Required, Range(0, 100)]
        public int Discount { get; set; }
        public int Count { get; set; }
    }
}
