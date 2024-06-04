using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.Product
{
    public class CreateProductVM
    {
        [MaxLength(16, ErrorMessage = "Lenght Error"),Required]
        public string Name { get; set; }

        [Required]
        public decimal SellPrice { get; set; }

        [Required]
        public decimal CostPrice { get; set; }

        [Range(0,100),Required]
        public int Discount { get; set; }

        [Required]
        public int StockCount { get; set; }


        [Required]
        public int[] CategoryIds { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        [Required]
        public IEnumerable<IFormFile> ImagesFile { get; set; }

        [Required]
        public float Raiting { get; set; }
    }
}
