using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccessLayer;
using Pronia.Extensions;
using Pronia.Models;
using Pronia.ViewModels.Slider;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Member")]
    public class SliderController(ProniaContext _context,IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index(int page=0)
        {
            int PageCount = 3; 
            double n = await _context.Sliders.CountAsync();
            ViewBag.MaxPage = Math.Ceiling((double)n/ PageCount);
            ViewBag.CurrentPage = page+1;
            ViewBag.PrevPage = page-1;
            var data = await _context.Sliders
                .Where(a => !a.isDeleted)
                .Skip(PageCount * page)
                .Take(PageCount)
                .Select(s => new GetSliderAdminVM
                {
                    Id = s.Id,
                    Title=s.Title,
                    SubTitle=s.SubTitle,
                    Discount=s.Discount,
                    ImageUrl=s.ImageUrl,
                    CreateTime = s.CreatedTime.ToString("dd MMM yyyy"),
                    UpdateTime = s.UpdateTime.Year > 1 ? s.UpdateTime.ToString("dd MMM yyyy") : "No Change",
                }).ToListAsync();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM svm)
        {
            if (svm.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Can Not Be Empty");
            if (!ModelState.IsValid)
                return View(svm);
            if (!svm.ImageFile.IsValidType("image"))
                ModelState.AddModelError("ImageFile", "Type Error");
            if (!svm.ImageFile.IsValidSize(200))
                ModelState.AddModelError("ImageFile", "Size Error");
            if (!ModelState.IsValid)
            {
                return View(svm);
            }
            string fileName = await svm.ImageFile.FileManageAsync(Path.Combine(_env.WebRootPath, "imgs", "sliders"));


            Slider sliders = new Slider
            {
                Title=svm.Title,
                SubTitle=svm.SubTitle,
                Discount=svm.Discount,
                ImageUrl=Path.Combine("imgs", "sliders", fileName),
            };
            
            await _context.Sliders.AddAsync(sliders);
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            var data = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (data == null) return NotFound();

            UpdateSliderVM swm = new UpdateSliderVM
            {
                Title = data.Title,
                SubTitle=data.SubTitle,
                Discount=data.Discount,
            };

            return View(swm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSliderVM swmVM)
        {
            if(id == null || id < 1) return BadRequest();
            
            var data = _context.Sliders.FirstOrDefault(s => s.Id == id);

            if (data == null) return NotFound();

            if (swmVM.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Can Not Be Empty");
            if (!ModelState.IsValid)
                return View(swmVM);
            if (!swmVM.ImageFile.IsValidType("image"))
                ModelState.AddModelError("ImageFile", "Type Error");
            if (!swmVM.ImageFile.IsValidSize(200))
                ModelState.AddModelError("ImageFile", "Size Error");
            if (!ModelState.IsValid)
            {
                return View(swmVM);
            }
            string fileName = await swmVM.ImageFile.FileManageAsync(Path.Combine(_env.WebRootPath, "imgs", "sliders"));


            data.Title = swmVM.Title;
            data.SubTitle = swmVM.SubTitle;
            data.Discount = swmVM.Discount;
            data.ImageUrl = Path.Combine("imgs", "sliders", fileName);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            var data = await _context.Sliders.FirstOrDefaultAsync(x => x.Id==id);

            if (data == null) return NotFound();

            data.ImageUrl.Delete(Path.Combine(_env.WebRootPath));

            _context.Remove(data);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ChengeVisbility(int? id)
        {
            var data = await _context.Sliders.FindAsync(id);

            if (data == null) return NotFound("Məlumat tapılmadı");

            data.isDeleted = !data.isDeleted;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
//