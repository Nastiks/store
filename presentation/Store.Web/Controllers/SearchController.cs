using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IJewelryRepository jewelryRepository;

        public SearchController(IJewelryRepository jewelryRepository)
        {
            this.jewelryRepository = jewelryRepository;
        }


        [HttpGet("/search")]
        public IActionResult Index(string query)
        {
            var jewelries = jewelryRepository.GetAllByTitle(query);

            return View(jewelries);
        }
    }
}
