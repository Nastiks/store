using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.App
{
    public class JewelryService
    {
        private readonly IJewelryRepository jewelryRepository;

        public JewelryService(IJewelryRepository jewelryRepository)
        {
            this.jewelryRepository = jewelryRepository;
        }

        public async Task<JewelryModel> GetByIdAsync(int id)
        {
            var jewelry = await jewelryRepository.GetByIdAsync(id);

            return Map(jewelry);
        }

        public async Task<IReadOnlyCollection<JewelryModel>> GetAllByQueryAsync(string query)
        {
            var jewelries = Jewelry.IsVendorCode(query)
                            ? await jewelryRepository.GetAllByVendorCodeAsync(query)
                            : await jewelryRepository.GetAllByTitleOrMaterialAsync(query);

            return jewelries.Select(Map)
                            .ToArray();
        }

        private JewelryModel Map(Jewelry jewelry)
        {
            return new JewelryModel
            {
                Id = jewelry.Id,
                VendorCode = jewelry.VendorCode,
                Title = jewelry.Title,
                Material = jewelry.Material,
                Description = jewelry.Description,
                Price = jewelry.Price,
            };
        }
    }
}