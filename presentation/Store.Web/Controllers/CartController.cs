using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IJewelryRepository jewelryRepository;
        private readonly IOrderRepository orderRepository;

        public CartController(IJewelryRepository jewelryRepository, IOrderRepository orderRepository)
        {
            this.jewelryRepository = jewelryRepository;
            this.orderRepository = orderRepository;
        }

        public IActionResult Add(int id)
        {
            Order order;
            Cart cart = new(id);

            if (!HttpContext.Session.TryGetCart(out cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }

            var jewelry = jewelryRepository.GetById(id);
            order.AddItem(jewelry, 1);
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Jewelry", new { id });
        }
    }
}
