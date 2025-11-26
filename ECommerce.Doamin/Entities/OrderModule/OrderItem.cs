using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Entities.OrderModule
{
    public class OrderItem:BaseEntity<int>
    {
        public ProductItemOrdered Product { get; set; } = default!;
        public decimal Price { get; set; }
        public string Name { get; set; } = default!;
    }
}
