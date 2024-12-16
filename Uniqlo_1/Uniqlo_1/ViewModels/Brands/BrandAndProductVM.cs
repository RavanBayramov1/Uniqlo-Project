using System.ComponentModel.DataAnnotations;

namespace Uniqlo_1.ViewModels.Brands
{
    public class BrandAndProductVM
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Name { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int Count { get; set; }
    }
}
