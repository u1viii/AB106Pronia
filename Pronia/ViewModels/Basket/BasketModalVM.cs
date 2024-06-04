namespace Pronia.ViewModels.Basket
{
    public class BasketModalVM
    {
        public IEnumerable<BasketItemVM> Items { get; set; }
        public decimal Total { get; set; }
    }
}
