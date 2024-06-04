using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DataAccessLayer;
using Pronia.Models;
using Pronia.ViewModels.Basket;

namespace Pronia.Controllers
{
    public class BasketController : Controller
    {
        private readonly ProniaContext _context;

        public BasketController(ProniaContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> basket=new List<BasketItemVM>();

            if (Request.Cookies["basket"] is not null){

                var cookies = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(Request.Cookies["basket"]);
                foreach (var cookieItem in cookies)
                {
                   Product product= await _context.Products.FirstOrDefaultAsync(p => p.Id == cookieItem.Id);
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
            return View(basket);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null || id<1) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p=>p.Id==id);
            if (product is null) return NotFound();

            List<BasketCookiesItemVM> basketVM=null;

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
                basketVM= JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(Request.Cookies["basket"]);
                BasketCookiesItemVM item= basketVM.FirstOrDefault(p => p.Id == product.Id);
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

            string json=JsonConvert.SerializeObject(basketVM);

            Response.Cookies.Append("basket", json);
           
            return RedirectToAction("index","home");
            
        }

        public IActionResult GetBasket()
        {

            return Content(Request.Cookies["basket"]);
        }
    }
}
