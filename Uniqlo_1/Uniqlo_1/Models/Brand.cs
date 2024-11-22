using System.ComponentModel.DataAnnotations;

namespace Uniqlo_1.Models
{
    public class Brand:BaseEntity
    {
        [MaxLength(64)]
        public string Name { get; set; }
        public ICollection<Product>? Products {  get; set; } 
    }
}
