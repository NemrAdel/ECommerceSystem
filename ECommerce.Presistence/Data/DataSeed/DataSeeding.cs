using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities;
using ECommerce.Doamin.Entities.OrderModule;
using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Presistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Presistence.Data.DataSeed
{
    public class DataSeeding : IDataSeed
    {
        private readonly StoreDbContext _dbContext;

        public DataSeeding(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                var HasProduct =await _dbContext.Products.AnyAsync();
                var HasProductBrand = await _dbContext.ProductBrands.AnyAsync();
                var HasProductType =await _dbContext.ProductTypes.AnyAsync();
                var HasDeliveryMethod = await _dbContext.Set<DeliveryMethod>().AnyAsync();
                if (HasProduct && HasProductBrand && HasProductType && HasDeliveryMethod)
                    return;
                if (!HasProductBrand)
                {
                    await SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                }
                if (!HasProductType)
                {
                    await SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);
                }
                _dbContext.SaveChanges();
                if (!HasProduct)
                {
                    await SeedDataFromJson<Product, int>("products.json", _dbContext.Products);
                }
                if (!HasDeliveryMethod)
                {
                    await SeedDataFromJson<DeliveryMethod,int>("delivery.json", _dbContext.Set<DeliveryMethod>());
                }
                _dbContext.SaveChanges();
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error in DataSeeding {err}");
            }
        }
        private async Task SeedDataFromJson<Tkey, T>(string fileName, DbSet<Tkey> dbset) where Tkey : BaseEntity<T>
        {
            var filePath = @"..\ECommerce.Presistence\Data\DataSeed\JsonFiles\" + fileName;
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Json file not found", filePath);

            try
            {
                //var data=File.ReadAllText(filePath);
                var dataStream=File.OpenRead(filePath);
                var data = await JsonSerializer.DeserializeAsync<List<Tkey>>(dataStream,new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if(data is not null)
                {
                    await dbset.AddRangeAsync(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SeedDataFromJson: {ex}");

            }
        }
    }
}
