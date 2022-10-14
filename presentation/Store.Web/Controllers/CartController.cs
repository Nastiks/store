using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IJewelryRepository jewelryRepository;

        public CartController(IJewelryRepository jewelryRepository)
        {
            this.jewelryRepository = jewelryRepository;
        }

        public IActionResult Add(int id)
        {
            var jewelry = jewelryRepository.GetById(id);
            Cart cart;
            if (!HttpContext.Session.TryGetCart(out cart))
            {
                cart = new Cart();
            }

            if (cart.Items.ContainsKey(id))
            {
                cart.Items[id]++;                
            }
            else
            {
                cart.Items[id] = 1;
            }

            cart.Amount += jewelry.Price;

            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Jewelry", new {id});
        }
    }
}
