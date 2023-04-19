using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {

        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context; 
        }   
        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders.Where(m => !m.SoftDelete).ToListAsync();

            return View(sliders);
        }
    }
}
