namespace Store.Memory
{
    public class JewelryRepository : IJewelryRepository
    {
        private readonly Jewelry[] jewelries = new[]
        {
            new Jewelry(1, "Vendor code 0000000001", "Epoxy resin and peonis", "Earrings with peonies",
                "Earrings made of jewelry resin with hypoallergenic accessories and pink peonies inside", 2000m),
            new Jewelry(2, "Vendor code 0000000002", "Epoxy resin and rose", "Rose pendant",
                "Pendant made of jewelry resin in the form of a drop with a red rose inside", 1200m),
            new Jewelry(3, "Vendor code 0000000003", "Pearl", "Pearl Necklace",
                "A necklace made of natural pearls that will adorn any woman", 3000m),
        };

        public Jewelry[] GetAllByIds(IEnumerable<int> jewelryIds)
        {
            var foundJewelries = from jewelry in jewelries
                                 join jewelryId in jewelryIds on jewelry.Id equals jewelryId
                                 select jewelry;

            return foundJewelries.ToArray();
        }

        public Jewelry[] GetAllByTitle(string titlePart)
        {
            return jewelries.Where(jewerly => jewerly.Title.Contains(titlePart))
                .ToArray();
        }

        public Jewelry[] GetAllByTitleOrMaterial(string query)
        {
            return jewelries.Where(jewelry => jewelry.Material.Contains(query)
            || jewelry.Title.Contains(query))
                .ToArray();
        }

        public Jewelry[] GetAllByVendorCode(string vendorCode)
        {
            return jewelries.Where(jewelry => jewelry.VendorCode == vendorCode)
                .ToArray();
        }

        public Jewelry GetById(int id)
        {
            return jewelries.Single(jewelry => jewelry.Id == id);
        }
    }
}