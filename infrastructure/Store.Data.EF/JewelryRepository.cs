using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Store.Data.EF
{
    class JewelryRepository : IJewelryRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public JewelryRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public Jewelry[] GetAllByIds(IEnumerable<int> jewelryIds)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            return dbContext.Jewelries
                            .Where(jewelry => jewelryIds.Contains(jewelry.Id))
                            .AsEnumerable()
                            .Select(Jewelry.Mapper.Map)
                            .ToArray();
        }

        public Jewelry[] GetAllByTitleOrMaterial(string titleOrMaterial)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            var parameter = new SqlParameter("@titleOrMaterial", titleOrMaterial);
            return dbContext.Jewelries
                            .FromSqlRaw("SELECT * FROM Jewelries WHERE CONTAINS((Material, Title), @titleOrMaterial)",
                                                  parameter)
                            .AsEnumerable()
                            .Select(Jewelry.Mapper.Map)
                            .ToArray();
        }

        public Jewelry[] GetAllByVendorCode(string vendorCode)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            if (Jewelry.TryFormatVendorCode(vendorCode, out string formattedVendorCode))
            {
                return dbContext.Jewelries
                           .Where(jewelry => jewelry.VendorCode == formattedVendorCode)
                           .AsEnumerable()
                           .Select(Jewelry.Mapper.Map)
                           .ToArray();
            }

            return new Jewelry[0];
        }

        public Jewelry GetById(int id)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            var dto = dbContext.Jewelries
                               .Single(jewelry => jewelry.Id == id);

            return Jewelry.Mapper.Map(dto);
        }
    }
}
