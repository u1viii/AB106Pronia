using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using Pronia.DataAccessLayer;
using Pronia.Models;
using Pronia.ViewModels.Basket;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Pronia.ViewComponents
{
    public class HeaderViewComponent(ProniaContext _context,UserManager<AppUser> _userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketItemVM> datas = new List<BasketItemVM>();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .ThenInclude(bi => bi.Product)
                    .FirstOrDefaultAsync(u => u.Id == UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value);

                foreach (var item in user.BasketItems)
                {
                    datas.Add(new BasketItemVM
                    {
                        Count = item.Count,
                        Id = item.ProductId,
                        Name = item.Product.Name,
                        ImageURL = item.Product.ImageUrl,
                        Price = item.Product.SellPrice


                    });
                }
            }
            else
            {
                if (Request.Cookies.Any(x => x.Key == "basket"))
                {
                    var basket = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(Request.Cookies["basket"] ?? "[]");
                    if (basket == null) throw new Exception();



                    
                        foreach (var cookieItem in basket)
                        {
                            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cookieItem.Id);
                            if (product is not null)
                            {
                                datas.Add(new BasketItemVM
                                {
                                    Id = product.Id,
                                    Name = product.Name,
                                    ImageURL = product.ImageUrl,
                                    Price = product.SellPrice,
                                    Count = cookieItem.Count

                                });
                            }
                        }
                    
                }
           
                
                //Ishlemir)
                //var datas = await _context.Products.Where(x => basket.Any(b=>b.Id==x.Id))
                //    .Select(x => new BasketItemVM
                //    {
                //        Id = x.Id,
                //        Name = x.Name,
                //        Count = basket.FirstOrDefault(b => b.Id == x.Id).Count,
                //        ImageURL = x.ImageUrl,
                //        Price = x.SellPrice * ((100 - x.Discount) / 100)
                //    }).ToListAsync();
               
            }
            //return View(new BasketModalVM
            //{
            //    Items = Enumerable.Empty<BasketItemVM>(),
            //    Total = 0
            //});
            return View(new BasketModalVM
            {
                Items = datas,
                Total = 0
            });
        }
    }
}
