using ECommerce.Doamin.Contracts;
using ECommerce.Presistence.Data.DbContexts;
using ECommerce.Presistence.IdentityData.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce.Api.Extensions
{
    public static class WebApplicationRegister
    {
        public static async Task<WebApplication> MigrateDataBaseAsync(this WebApplication app)
        {
            await using var scope =  app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any()) //change to sync again  to get iEnumerable
            {
                dbContext.Database.Migrate();
            }
            return app;
        }
        public static async Task<WebApplication> MigrateIdentityDataBaseAsync(this WebApplication app)
        {
            await using var scope =  app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any()) //change to sync again  to get iEnumerable
            {
                dbContext.Database.Migrate();
            }
            return app;
        }

        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            await using var scope =  app.Services.CreateAsyncScope();
            var dataSeed = scope.ServiceProvider.GetRequiredKeyedService<IDataSeed>("Default");
            await dataSeed.InitializeAsync();
            return app;
        }
        public static async Task<WebApplication> SeedIdentityData(this WebApplication app)
        {
            await using var scope =  app.Services.CreateAsyncScope();
            var dataSeed = scope.ServiceProvider.GetRequiredKeyedService<IDataSeed>("Identity");
            await dataSeed.InitializeAsync();
            return app;
        }
    }
}
