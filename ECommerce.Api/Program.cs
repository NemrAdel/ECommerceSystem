
using ECommerce.Api.Extensions;
using ECommerce.Doamin.Contracts;
using ECommerce.Presistence.Data.DataSeed;
using ECommerce.Presistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Register Dependancy Injection
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(); 
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDataSeed, DataSeeding>();
            #endregion

            var app = builder.Build();
            await app.MigrateDataBase();
            await app.SeedData();
            #region PipLine [MidleWares]
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync(); 
            #endregion
        }
    }
}
