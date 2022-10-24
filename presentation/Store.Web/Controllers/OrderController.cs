using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;
using System.Text.RegularExpressions;
using Store.Messages;
using Store.Contractors;
using Store.Web.Contractors;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IJewelryRepository jewelryRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly IEnumerable<IWebContractorService> webContractorServices;
        private readonly INotificationService notificationService;

        public OrderController(IJewelryRepository jewelryRepository,
                               IOrderRepository orderRepository,
                               IEnumerable<IDeliveryService> deliveryServices,
                               IEnumerable<IPaymentService> paymentServices,
                               IEnumerable<IWebContractorService> webContractorServices,
                               INotificationService notificationService)
        {
            this.jewelryRepository = jewelryRepository;
            this.orderRepository = orderRepository;
            this.deliveryServices = deliveryServices;
            this.paymentServices = paymentServices;
            this.webContractorServices = webContractorServices;
            this.notificationService = notificationService;
        }

        [HttpGet]
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

        [HttpPost]
        public IActionResult AddItem(int jewelryId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            var jewelry = jewelryRepository.GetById(jewelryId);

            if (order.Items.TryGet(jewelryId, out OrderItem orderItem))
            {
                orderItem.Count += count;
            }
            else
            {
                order.Items.Add(jewelry.Id, jewelry.Price, count);
            }

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Jewelry", new { id = jewelryId });

        }

        [HttpPost]
        public IActionResult UpdateItem(int jewelryId, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.Items.Get(jewelryId).Count = count;

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
                cart = new Cart(order.Id, 0, 0m);
            }
            return (order, cart);
        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);

            cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);

            HttpContext.Session.Set(cart);
        }

        [HttpPost]
        public IActionResult RemoveItem(int jewelryId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.Items.Remove(jewelryId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        [HttpPost]
        public IActionResult SendConfirmationCode(int id, string cellPhone)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            if (!IsValideCellPhone(cellPhone))
            {
                model.Errors["cellPhone"] = "Номер телефона не соответствует формату.";
                return View("Index", model);
            }

            int code = 1111; //random.Next(1000,10000)
            HttpContext.Session.SetInt32(cellPhone, code);
            notificationService.SendConfirmationCode(cellPhone, code);

            return View("Confirmation",
                new ConfirmationModel
                {
                    OrderId = id,
                    CellPhone = cellPhone
                });
        }

        private bool IsValideCellPhone(string cellPhone)
        {
            if (cellPhone == null)
            {
                return false;
            }

            cellPhone = cellPhone.Replace(" ", "")
                                 .Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
        }

        [HttpPost]
        public IActionResult Confirmate(int id, string cellPhone, int code)
        {
            int? storedCode = HttpContext.Session.GetInt32(cellPhone);

            if (storedCode == null)
            {
                return View("Confirmation",
                            new ConfirmationModel
                            {
                                OrderId = id,
                                CellPhone = cellPhone,
                                Errors = new Dictionary<string, string>
                                {
                                    { "code", "Пустой код, повторите отправку." }
                                },
                            }); ;
            }

            if (storedCode != code)
            {
                return View("Confirmation",
                           new ConfirmationModel
                           {
                               OrderId = id,
                               CellPhone = cellPhone,
                               Errors = new Dictionary<string, string>
                               {
                                    { "code", "Отличается от отправленного." }
                               },
                           }); ;
            }

            var order = orderRepository.GetById(id);
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            HttpContext.Session.Remove(cellPhone);

            var model = new DeliveryModel
            {
                OrderId = id,
                Methods = deliveryServices.ToDictionary(service => service.UniqueCode,
                                                      service => service.Title)
            };          

            return View("DeliveryMethod", model);
        }

        [HttpPost]
        public IActionResult StartDelivery(int id, string uniqueCode)
        {
            var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
            var order = orderRepository.GetById(id);

            var form = deliveryService.CreateForm(order);

            return View("DeliveryStep", form);
        }

        [HttpPost]
        public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);

            var form = deliveryService.MoveNextForm(id, step, values);

            if(form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Delivery = deliveryService.GetDelivery(form);
                orderRepository.Update(order);

                var model = new DeliveryModel
                {
                    OrderId = id,
                    Methods = paymentServices.ToDictionary(service => service.UniqueCode,
                                                      service => service.Title)
                };

                return View("PaymentMethod", model);
            }

            return View("DeliveryStep", form);
        }

        [HttpPost]
        public IActionResult StartPayment(int id, string uniqueCode)
        {
            var paymentService = paymentServices.Single(service => service.UniqueCode == uniqueCode);
            var order = orderRepository.GetById(id);

            var form = paymentService.CreateForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.UniqueCode == uniqueCode);
            
            if (webContractorService != null)
            {
                return Redirect(webContractorService.GetUri);
            }

            return View("PaymentStep", form);
        }

        [HttpPost]
        public IActionResult NextPayment(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.UniqueCode == uniqueCode);

            var form = paymentService.MoveNextForm(id, step, values);

            if (form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Payment = paymentService.GetPayment(form);
                orderRepository.Update(order);               

                return View("Finish");
            }

            return View("PaymentStep", form);
        }

        public IActionResult Finish()
        {
            HttpContext.Session.RemoveCart();

            return View();
        }
    }
}


