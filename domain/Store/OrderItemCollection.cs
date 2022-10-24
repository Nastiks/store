using System.Collections;

namespace Store
{
    public class OrderItemCollection : IReadOnlyCollection<OrderItem>
    {
        private readonly List<OrderItem> items;

        public OrderItemCollection(IEnumerable<OrderItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            this.items = new List<OrderItem>(items);
        }

        public int Count => items.Count;
        public IEnumerator<OrderItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (items as IEnumerable).GetEnumerator();
        }

        public OrderItem Get(int jewelryId)
        {
            if (TryGet(jewelryId, out OrderItem orderItem))
            {
                return orderItem;
            }

            throw new InvalidOperationException("Jewelry not found.");
        }

        public bool TryGet(int jewelryId, out OrderItem orderItem)
        {
            var index = items.FindIndex(item => item.JewelryId == jewelryId);
            if (index == -1)
            {
                orderItem = null;
                return false;                
            }

            orderItem = items[index];
            return true;
        }

        public OrderItem Add(int jewelryId, decimal price, int count)
        {
            if (TryGet(jewelryId, out OrderItem orderItem))
            {
                throw new InvalidOperationException("Jewelry already exsists.");
            }

            orderItem = new OrderItem(jewelryId, price, count);
            items.Add(orderItem);
            return orderItem;
        }

        public void Remove(int jewelryId)
        {
            items.Remove(Get(jewelryId));
        }
    }
}
