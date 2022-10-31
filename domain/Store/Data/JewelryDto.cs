namespace Store.Data
{
    public class JewelryDto
    {
        public int Id { get; set; }

        public string VendorCode { get; set; }

        public string? Material { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }
    }
}
