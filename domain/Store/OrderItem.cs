namespace Store
{
    public class OrderItem
    {
        public int JewelryId { get; }

        public int Count { get; }

        public decimal Price { get; }

        public OrderItem(int jewelryId, int count, decimal price)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("Count must be greater than zero.");
            }

            JewelryId = jewelryId;
            Count = count;
            Price = price;
        }
    }
}
