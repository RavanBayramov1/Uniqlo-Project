using System.ComponentModel.DataAnnotations;

namespace Uniqlo_1.ViewModels.Sliders
{
    public class SliderUpdateVM
    {
        [MaxLength(32, ErrorMessage = "Başlıq 32 simvoldan çox ola bilməz!"), Required]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
        public string? Link { get; set; }
        [Required(ErrorMessage = "Fayl seçilməyib!")]
        public string? ImageUrl { get; set; }
        public IFormFile File { get; set; }
    }
}
