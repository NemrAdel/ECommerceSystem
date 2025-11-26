using ECommerce.Doamin.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presistence.Data.Configurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(8,2)");
            builder.OwnsOne(x => x.Product, oe =>
            {
                oe.Property(x => x.ProductName).HasMaxLength(50);
            }
            );
        }
    }
}
