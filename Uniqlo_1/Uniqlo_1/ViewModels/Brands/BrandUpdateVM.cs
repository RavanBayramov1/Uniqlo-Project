using System.ComponentModel.DataAnnotations;

namespace Uniqlo_1.ViewModels.Brands
{
    public class BrandUpdateVM
    {
        [MaxLength(32, ErrorMessage = "Ad 32 simvoldan çox ola bilməz!"), Required]
        public string Name { get; set; } = null!;
    }
}
