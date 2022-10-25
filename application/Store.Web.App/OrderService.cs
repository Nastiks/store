using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using Store.Messages;

namespace Store.Web.App
{
    public class OrderService
    {
        private readonly IJewelryRepository jewelryRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        protected ISession Session => httpContextAccessor.HttpContext.Session;

        public OrderService(IJewelryRepository jewelryRepository,
                            IOrderRepository orderRepository,
                            INotificationService notificationService,
                            IHttpContextAccessor httpContextAccessor)
        {
            this.jewelryRepository = jewelryRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool TryGetModel(out OrderModel model)
        {
            if (TryGetOrder(out Order order))
            {
                model = Map(order);
                return true;
            }

            model = null;
            return false;
        }

        internal bool TryGetOrder(out Order order)
        {
            if (Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
                return true;
            }

            order = null;
            return false;
        }

        internal OrderModel Map(Order order)
        {
            var jewelries = GetJewelries(order);
            var items = from item in order.Items
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
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription = order.Payment?.Description
            };
        }

        internal IEnumerable<Jewelry> GetJewelries(Order order)
        {
            var jewelryIds = order.Items.Select(item => item.JewelryId);

            return jewelryRepository.GetAllByIds(jewelryIds);
        }

        public OrderModel AddJewelry(int jewelryId, int count)
        {
            if (count < 1)
            {
                throw new InvalidOperationException("Too few jewelries to add");
            }

            if (!TryGetOrder(out Order order))
            {
                order = orderRepository.Create();               
            }

            AddOrUpdateJewelry(order, jewelryId, count);
            UpdateSession(order);

            return Map(order);
        }

        internal void AddOrUpdateJewelry(Order order, int jewelryId, int count)
        {
            var jewelry = jewelryRepository.GetById(jewelryId);

            if (order.Items.TryGet(jewelryId, out OrderItem orderItem))
            {
                orderItem.Count += count;
            }
            else
            {
                order.Items.Add(jewelry.Id, jewelry.Price, count);
            }

            orderRepository.Update(order);
        }

        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }

        public OrderModel UpdateJewelry(int jewelryId, int count)
        {
            var order = GetOrder();
            order.Items.Get(jewelryId).Count = count;

            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }

        public OrderModel RemoveJewelry(int jewelryId)
        {
            var order = GetOrder();
            order.Items.Remove(jewelryId);

            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }

        public Order GetOrder()
        {
            if (TryGetOrder(out Order order))
            {
                return order;
            }

            throw new InvalidOperationException("Empty session.");
        }

        public OrderModel SendConfirmation(string cellPhone)
        {
            var order = GetOrder();
            var model = Map(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmationCode = 1111; // todo: random .Next(1000, 10000)
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmationCode);
                notificationService.SendConfirmationCode(formattedPhone, confirmationCode);
            }
            else
            {
                model.Errors["cellPhone"] = "Номер телефона не соответсвует формату +79999999999";
            }

            return model;
        }

        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

        internal bool TryFormatPhone(string cellPhone, out string formattedPhone)
        {
            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "ru");
                formattedPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
                return true;
            }
            catch (NumberParseException)
            {
                formattedPhone = null;
                return false;
            }            
        }

        public OrderModel ConfirmCellPhone(string cellPhone, int confirmationCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();

            if (storedCode == null)
            {
                model.Errors["cellPhone"] = "Что-то случилось. Попробуйте получить код ещё раз.";
                return model;
            }

            if (storedCode != confirmationCode)
            {
                model.Errors["confirmationCode"] = "Неверный код. Проверьте и попробуйте ещё раз.";
                return model;
            }

            var order = GetOrder();
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            Session.Remove(cellPhone);

            return Map(order);
        }

        public OrderModel SetDelivery(OrderDelivery delivery)
        {
            var order = GetOrder();
            order.Delivery = delivery;
            orderRepository.Update(order);

            return Map(order);
        }

        public OrderModel SetPayment(OrderPayment payment)
        {
            var order = GetOrder();
            order.Payment = payment;
            orderRepository.Update(order);
            Session.RemoveCart();

            return Map(order);
        }
    }
}
