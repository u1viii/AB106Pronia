namespace Pronia.ViewModels.Product;
public class GetProductVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal SellPrice { get; set; }
    public int Discount { get; set; }
    public bool IsInStock { get; set; }
    public string ImageUrl { get; set; }
    public float Raiting { get; set; }
}
