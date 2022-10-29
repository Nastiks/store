namespace Store
{
    public interface IJewelryRepository
    {
        Task<Jewelry[]> GetAllByTitleOrMaterialAsync(string titleOrMaterial);
        Task<Jewelry[]> GetAllByVendorCodeAsync(string vendorCode);
        Task<Jewelry> GetByIdAsync(int id);        
        Task<Jewelry[]> GetAllByIdsAsync(IEnumerable<int> jewelryIds);
    }
}
