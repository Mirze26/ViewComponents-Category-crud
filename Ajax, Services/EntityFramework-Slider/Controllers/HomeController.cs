using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EntityFramework_Slider.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        public HomeController(AppDbContext context,

           ICategoryService categoryService)

            
        {

            _categoryService = categoryService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {



            List<Slider> sliders = await _context.Sliders.ToListAsync();

            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();

            IEnumerable<Blog> blogs = await _context.Blogs.Where(m => !m.SoftDelete).ToListAsync();

            IEnumerable<Category> categories = await _categoryService.GetAll();

            IEnumerable<Product> products = await _context.Products.Include(m => m.Images).Where(m => !m.SoftDelete).ToListAsync();


            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Blogs = blogs,
                Categories = categories,
                Products = products
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) return BadRequest();

            Product? dbProduct = await GetProductById((int)id);

            if (dbProduct == null) return NotFound();

            List<BasketVm> basket = GetBasketDatas();

            BasketVm? existProduct = basket.FirstOrDefault(m => m.Id == dbProduct.Id);

            AddProductToBasket(existProduct, dbProduct, basket);



            int basketCount = basket.Sum(m => m.Count);

            return Ok(basketCount);
        }

        private async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }


        private List<BasketVm?> GetBasketDatas()
        {


            List<BasketVm> basket;

            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVm>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVm>();
            }

            return basket;

        }


        private void AddProductToBasket(BasketVm? existProduct, Product product, List<BasketVm> basket)
        {
            if (existProduct == null)
            {
                basket?.Add(new BasketVm
                {
                    Id = product.Id,
                    Count = 1
                });

            }
            else
            {
                existProduct.Count++;
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
        }

    }
}