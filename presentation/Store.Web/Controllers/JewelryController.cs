using Microsoft.AspNetCore.Mvc;
using Store.Web.App;

namespace Store.Web.Controllers
{
    public class JewelryController : Controller
    {
        private readonly JewelryService jewelryService;

        public JewelryController(JewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
        }

        public IActionResult Index(int id)
        {
            var model = jewelryService.GetById(id);

            return View(model);
        }
    }
}
