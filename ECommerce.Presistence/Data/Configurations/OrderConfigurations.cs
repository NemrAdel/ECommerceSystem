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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.SubTotal).HasColumnType("decimal(8,2)");
            builder.OwnsOne(x => x.Address, OE => {
                OE.Property(x => x.FirsName).HasMaxLength(100);
                OE.Property(x => x.LastName).HasMaxLength(100);
                OE.Property(x => x.City).HasMaxLength(100);
                OE.Property(x => x.Street).HasMaxLength(100);
                OE.Property(x => x.Country).HasMaxLength(100);
            };
        }
    }
}
