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

        public int TotalCount
        {
            get { return items.Sum(item => item.Count); }
        }

        public decimal TotalPrice
        {
            get { return items.Sum(item => item.Price * item.Count); }
        }

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Id = id;

            this.items = new List<OrderItem>(items);
        }

        public void AddItem(Jewelry jewelry, int count)
        {
            if (jewelry == null)
            {
                throw new ArgumentNullException(nameof(jewelry));
            }

            var item = items.SingleOrDefault(x => x.JewelryId == jewelry.Id);

            if (item == null)
            {
                items.Add(new OrderItem(jewelry.Id, count, jewelry.Price));
            }
            else
            {
                items.Remove(item);
                items.Add(new OrderItem(jewelry.Id, item.Count + count, jewelry.Price));
            }            
        }
    }
}
