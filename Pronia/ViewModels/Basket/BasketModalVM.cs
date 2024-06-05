namespace Pronia.ViewModels.Basket
{
    public class BasketModalVM
    {
        public required IEnumerable<BasketItemVM> Items { get; set; }
        public decimal Total { get; set; }
        public int Count { get; set; }
    }
}
