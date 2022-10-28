using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Store.Data.EF
{
    class DbContextFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public DbContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public StoreDbContext Create(Type repositoryType)
        {
            var services = httpContextAccessor.HttpContext.RequestServices;

            var dbContext = services.GetService<Dictionary<Type, StoreDbContext>>();
            if (!dbContext.ContainsKey(repositoryType))
            {
                dbContext[repositoryType] = services.GetService<StoreDbContext>();
            }
            return dbContext[repositoryType];
        }
    }
}
