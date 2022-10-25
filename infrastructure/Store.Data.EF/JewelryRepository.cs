namespace Store.Data.EF
{
    class JewelryRepository : IJewelryRepository
    {
        public Jewelry[] GetAllByIds(IEnumerable<int> jewelryIds)
        {
            throw new NotImplementedException();
        }

        public Jewelry[] GetAllByTitleOrMaterial(string titlePartOrMaterial)
        {
            throw new NotImplementedException();
        }

        public Jewelry[] GetAllByVendorCode(string vendorCode)
        {
            throw new NotImplementedException();
        }

        public Jewelry GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
