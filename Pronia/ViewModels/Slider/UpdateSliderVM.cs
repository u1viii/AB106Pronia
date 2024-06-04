using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.Slider
{
    public class UpdateSliderVM
    {
        [MaxLength(32, ErrorMessage = "Başlıq 32 simvoldun artıq ola bilməz"), Required]
        public string Title { get; set; }

        [MaxLength(64, ErrorMessage = "Description 64 simvoldun artıq ola bilməz"), Required]
        public string SubTitle { get; set; }

        [Range(0, 100)]
        public int Discount { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
