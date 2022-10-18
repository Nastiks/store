using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IJewelryRepository jewelryRepository;
        private readonly IOrderRepository orderRepository;

        public OrderController(IJewelryRepository jewelryRepository, IOrderRepository orderRepository)
        {
            this.jewelryRepository = jewelryRepository;
            this.orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                OrderModel model = Map(order);

                return View(model);
            }

            return View("Empty");
        }

        private OrderModel Map(Order order)
        {
            var jewelryIds = order.Items.Select(item => item.JewelryId);
            var jewelries = jewelryRepository.GetAllByIds(jewelryIds);
            var itemModels = from item in order.Items
                             join jewelry in jewelries on item.JewelryId equals jewelry.Id
                             select new OrderItemModel
                             {
                                 JewelryId = jewelry.Id,
                                 Title = jewelry.Title,
                                 Material = jewelry.Material,
                                 Price = item.Price,
                                 Count = item.Count,
                             };
            return new OrderModel
            {
                Id = order.Id,
                Items = itemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
            };
        }

        public IActionResult AddItem(int jewelryId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            var jewelry = jewelryRepository.GetById(jewelryId);

            order.AddOrUpdateItem(jewelry, count);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Jewelry", new { id = jewelryId });

        }

        [HttpPost]
        public IActionResult UpdateItem(int jewelryId, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.GetItem(jewelryId).Count = count;

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }        

        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }
            return (order, cart);
        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;

            HttpContext.Session.Set(cart);
        }       

        public IActionResult RemoveItem(int jewelryId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.RemoveItem(jewelryId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }
    }
}
