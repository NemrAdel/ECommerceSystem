using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities;
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
        public void Initialize()
        {
            try
            {
                var HasProduct = _dbContext.Products.Any();
                var HasProductBrand = _dbContext.ProductBrands.Any();
                var HasProductType = _dbContext.ProductTypes.Any();
                if (HasProduct && HasProductBrand && HasProductType)
                    return;
                if (!HasProductBrand)
                {
                    SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                }
                if (!HasProductType)
                {
                    SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);
                }
                _dbContext.SaveChanges();
                if (!HasProduct)
                {
                    SeedDataFromJson<Product, int>("product.json", _dbContext.Products);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error in DataSeeding {err}");
            }
        }
        private void SeedDataFromJson<Tkey, T>(string fileName, DbSet<Tkey> dbset) where Tkey : BaseEntity<T>
        {
            var filePath = @"..\ECommerce.Presistence\Data\DataSeed\Data\" + fileName;
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Json file not found", filePath);

            try
            {
                //var data=File.ReadAllText(filePath);
                var dataStream=File.OpenRead(filePath);
                var data = JsonSerializer.Deserialize<List<Tkey>>(dataStream,new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if(data is not null)
                {
                    dbset.AddRange(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SeedDataFromJson: {ex}");

            }
        }
    }
}
