namespace Pronia.Models
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product? product { get; set; }
    }
}
