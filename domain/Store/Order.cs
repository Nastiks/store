namespace Store
{
    public class Order
    {
        public int Id { get; }

        private List<OrderItem> items;

        public IReadOnlyCollection<OrderItem> Items
        {
            get { return items; }
        }

        public string CellPhone { get; set; }

        public OrderDelivery Delivery { get; set; }

        public OrderPayment Payment { get; set; }

        public int TotalCount => items.Sum(item => item.Count);        

        public decimal TotalPrice => items.Sum(item => item.Price * item.Count)
                                     + (Delivery?.Amount ?? 0m);
        
        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Id = id;

            this.items = new List<OrderItem>(items);
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

        public void AddOrUpdateItem(Jewelry jewelry, int count)
        {
            if (jewelry == null)
            {
                throw new ArgumentNullException(nameof(jewelry));
            }

            int index = items.FindIndex(item => item.JewelryId == jewelry.Id);
            if(index == -1)
            {
                items.Add(new OrderItem(jewelry.Id, count, jewelry.Price));
            }
            else
            {
                items[index].Count += count;
            }     
        }       

        public void RemoveItem(int jewelryId)
        {           

            int index = items.FindIndex(item => item.JewelryId == jewelryId);

            if(index == -1)
            {
                ThrowJewelryException("Order does not contain specified item.", jewelryId);
            }

            items.RemoveAt(index);            
        }

        private void ThrowJewelryException(string message, int jewelryId)
        {
            var exception = new InvalidOperationException(message);

            exception.Data["JewelryId"] = jewelryId;

            throw exception;
        }
    }
}
