namespace Store
{
    public class Order
    {
        public int Id { get; }

        private List<OrderItem> items;

        public OrderItemCollection Items { get; }
       
        public string CellPhone { get; set; }

        public OrderDelivery Delivery { get; set; }

        public OrderPayment Payment { get; set; }

        public int TotalCount => Items.Sum(item => item.Count);        

        public decimal TotalPrice => Items.Sum(item => item.Price * item.Count)
                                     + (Delivery?.Amount ?? 0m);
        
        public Order(int id, IEnumerable<OrderItem> items)
        {
            Id = id;
            Items = new OrderItemCollection(items);
        }

        public OrderItem GetItem(int jewelryId)
        {
            int index = items.FindIndex(item => item.JewelryId == jewelryId);

            if (index == -1)
            {
                ThrowJewelryException("Jewelry not found.", jewelryId);
            }

            return items[index];
        }       

        private void ThrowJewelryException(string message, int jewelryId)
        {
            var exception = new InvalidOperationException(message);

            exception.Data["JewelryId"] = jewelryId;

            throw exception;
        }
    }
}
