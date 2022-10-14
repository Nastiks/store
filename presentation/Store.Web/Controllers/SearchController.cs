using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    
    public class SearchController : Controller
    {
        private readonly JewelryService jewelryService;

        public SearchController(JewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
        }

        public IActionResult Index(string query)
        {
            var jewelries = jewelryService.GetAllByQuery(query);

            return View("Index", jewelries);
        }
    }
}
