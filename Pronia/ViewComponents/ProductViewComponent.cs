using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccessLayer;
using Pronia.ViewModels.Product;

namespace Pronia.ViewComponents
{
    public class ProductViewComponent(ProniaContext _context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products.Select(p=> new GetProductVM
            {
                Discount = p.Discount,
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                IsInStock = p.StockCount > 0,
                Name = p.Name,
                Raiting = p.Raiting,
                SellPrice = p.SellPrice,
            }).ToListAsync();
            return View(products);
        }
    }
}
