using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccessLayer;
using Pronia.Extensions;
using Pronia.ViewModels.Category;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Member")]
    public class CategoryController(ProniaContext _context) : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            var data = await _context.Categories
                .Select(s => new GetCategoryVM
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToListAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Create(CreateCategoryVM vm)
        {

            if(!ModelState.IsValid) 
            {
                return View(vm);
            }

            await _context.Categories.AddAsync(new Models.Category
            {
                Name = vm.Name,
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            var data = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (data == null) return NotFound();

            _context.Remove(data);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
