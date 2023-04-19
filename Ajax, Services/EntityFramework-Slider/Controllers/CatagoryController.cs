using EntityFramework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EntityFramework_Slider.Controllers
{
    [Area("Admin")]
    public class CatagoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CatagoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task <IActionResult> Index()
        {
            return View(await _categoryService.GetAll());
        }

    }
}
