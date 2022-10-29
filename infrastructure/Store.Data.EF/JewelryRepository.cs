using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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

            var dtos = dbContext.Jewelries
                                .Where(jewelry => jewelryIds.Contains(jewelry.Id))
                                .ToArray();

            return dtos.Select(Jewelry.Mapper.Map)
                       .ToArray();
        }

        public Jewelry[] GetAllByVendorCode(string vendorCode)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            if (Jewelry.TryFormatVendorCode(vendorCode, out string formattedVendorCode))
            {
                var dtos = dbContext.Jewelries
                                .Where(jewelry => jewelry.VendorCode == formattedVendorCode)
                                .ToArray();

                return dtos.Select(Jewelry.Mapper.Map)
                           .ToArray();
            }

            return new Jewelry[0];
        }

        public Jewelry[] GetAllByTitleOrMaterial(string titleOrMaterial)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            var parameter = new SqlParameter("@titleOrMaterial", titleOrMaterial);
            var dtos = dbContext.Jewelries
                                .FromSqlRaw("SELECT * FROM Jewelries WHERE CONTAINS((Material, Title), @titleOrMaterial)",
                                            parameter)
                                 .ToArray();

            return dtos.Select(Jewelry.Mapper.Map)
                       .ToArray();
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
