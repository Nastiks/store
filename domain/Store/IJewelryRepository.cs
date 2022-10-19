namespace Store
{
    public interface IJewelryRepository
    {
        Jewelry[] GetAllByVendorCode(string vendorCode);

        Jewelry[] GetAllByTitleOrMaterial(string titlePartOrMaterial);

        Jewelry GetById(int id);
        Jewelry[] GetAllByIds(IEnumerable<int> jewelryIds);
    }
}
