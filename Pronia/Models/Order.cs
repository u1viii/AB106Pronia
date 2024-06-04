namespace Pronia.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchasedAt { get; set; }
        public bool? Status { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}
