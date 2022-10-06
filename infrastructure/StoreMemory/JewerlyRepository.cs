using System;
using System.Linq;

namespace Store.Memory
{
    public class JewelryRepository : IJewelryRepository
    {
        private readonly Jewelry[] jewelries = new[]
        {
            new Jewelry(1, "Earrings with peonies"),
            new Jewelry(2, "Rose pendant"),
            new Jewelry(3, "Pearl Necklace"),
        };

        public Jewelry[] GetAllByTitle(string titlePart)
        {
            return jewelries.Where(jewerly => jewerly.Title.Contains(titlePart))
                .ToArray();
        }

    }
}