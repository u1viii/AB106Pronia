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
            return View();
        }
    }
}
