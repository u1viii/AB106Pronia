namespace Pronia.ViewModels.Basket
{
    public class BasketItemVM
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
    }
}
