using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DataAccessLayer;
using Pronia.Models;
using Pronia.ViewModels.Basket;
using System.Security.Claims;

namespace Pronia.Controllers
{
    public class BasketController : Controller
    {
        private readonly ProniaContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(ProniaContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> basket=new List<BasketItemVM>();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .ThenInclude(bi=>bi.Product)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value);

                foreach (var item in user.BasketItems)
                {
                    basket.Add(new BasketItemVM
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
                if (Request.Cookies["basket"] is not null)
                {

                    var cookies = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(Request.Cookies["basket"]);
                    foreach (var cookieItem in cookies)
                    {
                        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cookieItem.Id);
                        if (product is not null)
                        {
                            basket.Add(new BasketItemVM
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
            }

           
            return View(basket);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null || id<1) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p=>p.Id==id);
            if (product is null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                //var user = await _userManager.FindByNameAsync(User.Identity.Name);

                //var user = await _userManager.Users.Include(u => u.BasketItems).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                var user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .FirstOrDefaultAsync(u => u.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var item=user.BasketItems.FirstOrDefault(bi=>bi.Id==id);
                if(item is null)
                {
                    user.BasketItems.Add(new BasketItem
                    {
                        Product = product,
                        Price = product.SellPrice,
                        Count = 1,
                        OrderId = null

                    });
                }
                else
                {
                    item.Count++;
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                List<BasketCookiesItemVM> basketVM = null;

                if (Request.Cookies["basket"] is null)
                {
                    basketVM = new List<BasketCookiesItemVM>();
                    BasketCookiesItemVM basketCookiesItemVM = new BasketCookiesItemVM
                    {
                        Id = product.Id,
                        Count = 1
                    };
                    basketVM.Add(basketCookiesItemVM);

                }
                else
                {
                    basketVM = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(Request.Cookies["basket"]);
                    BasketCookiesItemVM item = basketVM.FirstOrDefault(p => p.Id == product.Id);
                    if (item is null)
                    {
                        BasketCookiesItemVM basketCookiesItemVM = new BasketCookiesItemVM
                        {
                            Id = product.Id,
                            Count = 1
                        };
                        basketVM.Add(basketCookiesItemVM);
                    }
                    else
                    {
                        item.Count++;
                    }

                }

                string json = JsonConvert.SerializeObject(basketVM);

                Response.Cookies.Append("basket", json);
            }

           
           
            return RedirectToAction("index","home");
            
        }

        public IActionResult GetBasket()
        {

            return Content(Request.Cookies["basket"]);
        }
    }
}
