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

        public async Task<(bool hasValue, OrderModel model)> TryGetModelAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();
            if (hasValue)
            {                
                return (true, await MapAsync(order));
            }
            return (false, null);
        }          

        internal async Task<(bool hasValue, Order order)> TryGetOrderAsync()
        {
            if (Session.TryGetCart(out Cart cart))
            {
                var order = await orderRepository.GetByIdAsync(cart.OrderId);
                return (true, order);
            }
            return (false, null);
        }        

        internal async Task<OrderModel> MapAsync(Order order)
        {
            var jewelries = await GetJewelriesAsync(order);
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

        internal async Task<IEnumerable<Jewelry>> GetJewelriesAsync(Order order)
        {
            var jewelryIds = order.Items.Select(item => item.JewelryId);

            return await jewelryRepository.GetAllByIdsAsync(jewelryIds);
        }

        public async Task<OrderModel> AddJewelryAsync(int jewelryId, int count)
        {
            if (count < 1)
            {
                throw new InvalidOperationException("Too few jewelries to add");
            }

            var (hasValue, order) = await TryGetOrderAsync();

            if (!hasValue)
            {
                order = await orderRepository.CreateAsync();
            }

            await AddOrUpdateJewelryAsync(order, jewelryId, count);
            UpdateSession(order);

            return await MapAsync(order);
        }               

        internal async Task AddOrUpdateJewelryAsync(Order order, int jewelryId, int count)
        {
            var jewelry = await jewelryRepository.GetByIdAsync(jewelryId);
            
            if (order.Items.TryGet(jewelryId, out OrderItem orderItem))
            {
                orderItem.Count += count;
            }
            else
            {
                order.Items.Add(jewelry.Id, jewelry.Price, count);
            }

            await orderRepository.UpdateAsync(order);
        }

        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }        

        public async Task<OrderModel> UpdateJewelryAsync(int jewelryId, int count)
        {
            var order = await GetOrderAsync();
            order.Items.Get(jewelryId).Count = count;

            await orderRepository.UpdateAsync(order);
            UpdateSession(order);

            return await MapAsync(order);
        }       

        public async Task<OrderModel> RemoveJewelryAsync(int jewelryId)
        {
            var order = await GetOrderAsync();
            order.Items.Remove(jewelryId);

            await orderRepository.UpdateAsync(order);
            UpdateSession(order);

            return await MapAsync(order);
        }
        
        public async Task<Order> GetOrderAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();

            if (hasValue)
            {
                return order;
            }

            throw new InvalidOperationException("Empty session.");
        }       

        public async Task<OrderModel> SendConfirmationAsync(string cellPhone)
        {
            var order = await GetOrderAsync();
            var model = await MapAsync(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmationCode = 1111; // todo: random .Next(1000, 10000)
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmationCode);
                await notificationService.SendConfirmationCodeAsync(formattedPhone, confirmationCode);
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

        public async Task<OrderModel> ConfirmCellPhoneAsync(string cellPhone, int confirmationCode)
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

            var order = await GetOrderAsync();
            order.CellPhone = cellPhone;
            await orderRepository.UpdateAsync(order);

            Session.Remove(cellPhone);

            return await MapAsync(order);
        }       

        public async Task<OrderModel> SetDeliveryAsync(OrderDelivery delivery)
        {
            var order = await GetOrderAsync();
            order.Delivery = delivery;
            await orderRepository.UpdateAsync(order);

            return await MapAsync(order);
        }        

        public async Task<OrderModel> SetPaymentAsync(OrderPayment payment)
        {
            var order = await GetOrderAsync();
            order.Payment = payment;
            await orderRepository.UpdateAsync(order);
            Session.RemoveCart();

            await notificationService.StartProcessAsync(order);

            return await MapAsync(order);
        }
    }
}
