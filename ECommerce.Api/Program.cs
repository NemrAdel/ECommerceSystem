using AutoMapper;
using ECommerce.Api.Extensions;
using ECommerce.Doamin.Contracts;
using ECommerce.Presistence.Data.DataSeed;
using ECommerce.Presistence.Data.DbContexts;
using ECommerce.Presistence.Repositories;
using ECommerce.Service;
using ECommerce.Service.MappingProfiles;
using ECommerce.Services.Abstraction;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

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
            //builder.Services.AddOpenApi(); 
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddScoped<IDataSeed, DataSeeding>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddTransient<ProductPictureurlResolver>();
            //builder.Services.AddScoped<IMapper, Mapper>();
            builder.Services.AddAutoMapper(typeof(ServiceAssemblyReference).Assembly);
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });
            #endregion

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            var app = builder.Build();
            await app.MigrateDataBase();
            await app.SeedData();
            #region PipLine [MidleWares]

            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync(); 
            #endregion
        }
    }
}
