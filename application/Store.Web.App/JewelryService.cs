using Store.Web.App;

namespace Store
{
    public class JewelryService
    {
        private readonly IJewelryRepository jewelryRepository;

        public JewelryService(IJewelryRepository jewelryRepository)
        {
            this.jewelryRepository = jewelryRepository;
        }

        public JewelryModel GetById(int id)
        {
            var jewelry = jewelryRepository.GetById(id);

            return Map(jewelry);
        }

        public IReadOnlyCollection<JewelryModel> GetAllByQuery(string query)
        {
            var jewelries = Jewelry.IsVendorCode(query)
                            ? jewelryRepository.GetAllByVendorCode(query)
                            : jewelryRepository.GetAllByTitleOrMaterial(query);

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
