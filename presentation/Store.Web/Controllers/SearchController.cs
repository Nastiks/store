using Microsoft.AspNetCore.Mvc;
using Store.Web.App;

namespace Store.Web.Controllers
{    
    public class SearchController : Controller
    {
        private readonly JewelryService jewelryService;

        public SearchController(JewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
        }        

        public async Task<IActionResult> Index(string query)
        {
            var jewelries = await jewelryService.GetAllByQueryAsync(query);

            return View("Index", jewelries);
        }
    }
}
