using System;
using System.Linq;

namespace Store.Memory
{
    public class JewelryRepository : IJewelryRepository
    {
        private readonly Jewelry[] jewelries = new[]
        {
            new Jewelry(1, "Vendor code 0000000001", "Epoxy resin and peonis", "Earrings with peonies"),
            new Jewelry(2, "Vendor code 0000000002", "Epoxy resin and rose", "Rose pendant"),
            new Jewelry(3, "Vendor code 0000000003", "Pearl", "Pearl Necklace"),
        };

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
    }
}