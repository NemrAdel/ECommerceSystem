using AutoMapper;
using ECommerce.Api.CustomeMiddleWares;
using ECommerce.Api.Extensions;
using ECommerce.Api.Factories;
using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.IdentityModule;
using ECommerce.Presistence.Data.DataSeed;
using ECommerce.Presistence.Data.DbContexts;
using ECommerce.Presistence.IdentityData.DataSeed;
using ECommerce.Presistence.IdentityData.DbContext;
using ECommerce.Presistence.Repositories;
using ECommerce.Service;
using ECommerce.Service.MappingProfiles;
using ECommerce.Services.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;



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

            // database security models
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });


            builder.Services.AddKeyedScoped<IDataSeed, DataSeeding>("Default");
            builder.Services.AddKeyedScoped<IDataSeed, IdentityDataIntializer>("Identity");
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

            builder.Services.AddScoped<IBasketService,BasketService>();
            builder.Services.AddScoped<ICacheRepository,CacheRepository>();
            builder.Services.AddScoped<ICacheService,CacheService>();
            builder.Services.AddScoped<IAuthenticationService,AuthenticationService>();
            builder.Services.AddScoped<IOrderService,OrderService>();
            builder.Services.AddScoped<ISecurityRepository<Address>,SecurityRepository<Address>>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme; //use the same schema whicj generate the token (see the token)
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme; // use if it invalid or not (if not valid return 401)
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true; // save in httpcontext to retrieve any time if it valid
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer=true,
                    ValidateAudience=true,
                    ValidateLifetime=true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!))
                };

            });

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //.AddEntityFrameWorkStores<StoreIdentityDbContext>();//Take the user and role => we didn't modify role 

            builder.Services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>(); // light weight for user only and roles

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse
            );
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
            await app.MigrateDataBaseAsync();
            await app.MigrateIdentityDataBaseAsync();
            await app.SeedDataAsync();
            await app.SeedIdentityData();



            #region PipLine [MidleWares]



            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionHandlerMiddleWare>(); // using custome middleware
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            await app.RunAsync(); 
            #endregion
        }
    }
}
