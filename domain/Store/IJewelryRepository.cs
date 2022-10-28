namespace Store
{
    public interface IJewelryRepository
    {
        Jewelry[] GetAllByVendorCode(string vendorCode);

        Jewelry[] GetAllByTitleOrMaterial(string titleOrMaterial);

        Jewelry GetById(int id);
        Jewelry[] GetAllByIds(IEnumerable<int> jewelryIds);
    }
}
