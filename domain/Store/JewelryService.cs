using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class JewelryService
    {
        private readonly IJewelryRepository jewelryRepository;

        public JewelryService(IJewelryRepository jewelryRepository)
        {
            this.jewelryRepository = jewelryRepository;
        }

        public Jewelry[] GetAllByQuery(string query)
        {
            if(Jewelry.IsVendorCode(query))
            {
                return jewelryRepository.GetAllByVendorCode(query);
            }

            return jewelryRepository.GetAllByTitleOrMaterial(query);
        }
    }
}
