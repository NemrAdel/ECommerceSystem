using ECommerce.Doamin.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presistence.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(200);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.PictureUrl).HasMaxLength(200);
            builder.Property(p => p.Price).HasPrecision(18, 2);
            builder.HasOne(p => p.ProductType).WithMany().HasForeignKey(p => p.ProductTypeId);
            builder.HasOne(p => p.ProductBrand).WithMany().HasForeignKey(p => p.ProductBrandId);
        }
    }
}
