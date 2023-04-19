using EntityFramework_Slider.Data;
using EntityFramework_Slider.Services.Interface;
using EntityFramework_Slider.ViewModels;
using Newtonsoft.Json;

namespace EntityFramework_Slider.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LayoutService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public LayoutVM GetSettingDatas()
        {
            Dictionary<string,string> settings = _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<BasketVm> basketDatas = GetBasketDatas();

            int count = basketDatas.Sum(b => b.Count);

            LayoutVM model = new()
            {
                Settings= settings,
                BasketCount= count,
            };

            return model;
        }

        private List<BasketVm?> GetBasketDatas()
        {
            List<BasketVm> basket;

            if (_httpContextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVm>>(_httpContextAccessor.HttpContext.Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVm>();
            }

            return basket;
        }
    } 
    
}
