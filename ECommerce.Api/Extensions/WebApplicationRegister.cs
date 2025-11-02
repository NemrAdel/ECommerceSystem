namespace ECommerce.Api.Extensions
{
    public static class WebApplicationRegister
    {
        public static WebApplication MigrateDataBase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            //var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            //if (dbContext.Database.GetPendingMigrations().Any())
            //{
            //    dbContext.Database.Migrate();
            //}
            return app;
        }

        public static WebApplication SeedData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataSeed = scope.ServiceProvider.GetRequiredService<Doamin.Contracts.IDataSeed>();
            dataSeed.Initialize();
            return app;
        }
    }
}
