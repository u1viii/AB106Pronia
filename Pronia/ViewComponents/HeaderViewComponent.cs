using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DataAccessLayer;
using Pronia.ViewModels.Basket;
using System.Text.Json.Serialization;

namespace Pronia.ViewComponents
{
    public class HeaderViewComponent(ProniaContext _context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (Request.Cookies.Any(x => x.Key == "basket"))
            {
                var basket = JsonConvert.DeserializeObject<IEnumerable<BasketCookiesItemVM>>(Request.Cookies["basket"] ?? "[]");
                if (basket == null) throw new Exception();
                var datas = await _context.Products.Where(x => basket.Select(b => b.Id).Contains(x.Id))
                    .Select(x => new BasketItemVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Count = basket.FirstOrDefault(b => b.Id == x.Id).Count,
                        ImageURL = x.ImageUrl,
                        Price = x.SellPrice * ((100 - x.Discount) / 100)
                    }).ToListAsync();
                return View(new BasketModalVM
                {
                    Items = datas,
                    Total = 0
                });
            }
            return View(new BasketModalVM
            {
                Items = Enumerable.Empty<BasketItemVM>(),
                Total = 0
            });
        }
    }
}
