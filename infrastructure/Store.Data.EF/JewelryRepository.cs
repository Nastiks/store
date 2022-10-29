using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Data.EF
{
    class JewelryRepository : IJewelryRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public JewelryRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<Jewelry[]> GetAllByIdsAsync(IEnumerable<int> jewelryIds)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            var dtos = await dbContext.Jewelries
                                      .Where(jewelry => jewelryIds.Contains(jewelry.Id))
                                      .ToArrayAsync();

            return dtos.Select(Jewelry.Mapper.Map)
                       .ToArray();
        }       

        public async Task<Jewelry[]> GetAllByTitleOrMaterialAsync(string titleOrMaterial)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            var parameter = new SqlParameter("@titleOrMaterial", titleOrMaterial);
            var dtos = await dbContext.Jewelries
                                      .FromSqlRaw("SELECT * FROM Jewelries WHERE CONTAINS((Material, Title), @titleOrMaterial)",
                                                  parameter)
                                      .ToArrayAsync();

            return dtos.Select(Jewelry.Mapper.Map)
                       .ToArray();            
        }

        public async Task<Jewelry[]> GetAllByVendorCodeAsync(string vendorCode)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            if (Jewelry.TryFormatVendorCode(vendorCode, out string formattedVendorCode))
            {
                var dtos = await dbContext.Jewelries
                                               .Where(jewelry => jewelry.VendorCode == formattedVendorCode)
                                               .ToArrayAsync();

                return dtos.Select(Jewelry.Mapper.Map)
                           .ToArray();                
            }

            return new Jewelry[0];
        }

        public async Task<Jewelry> GetByIdAsync(int id)
        {
            var dbContext = dbContextFactory.Create(typeof(JewelryRepository));

            var dto = await dbContext.Jewelries
                                     .SingleAsync(jewelry => jewelry.Id == id);

            return Jewelry.Mapper.Map(dto);
        }
    }
}
