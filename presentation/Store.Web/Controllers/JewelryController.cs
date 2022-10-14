using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    public class JewelryController : Controller
    {
        private readonly IJewelryRepository jewelryRepository;

        public JewelryController(IJewelryRepository jewelryRepository)
        {
            this.jewelryRepository = jewelryRepository;
        }

        public IActionResult Index(int id)
        {
            Jewelry jewelry = jewelryRepository.GetById(id);

            return View(jewelry);
        }
    }
}
