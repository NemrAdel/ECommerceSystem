using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Entities.OrderModule
{
    public class Order:BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now; // offset mean if the server on another country
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; } = default!; 
        public OrderAddress Address { get; set; } = default!; 
        public DeliveryMethod DeliveryMethod { get; set; } = default!; 
        public int DeliveryMethodId { get; set; } = default!; // FK
        public ICollection<OrderItem> Items { get; set; } = [];
        public decimal SubTotal { get; set; } // quantity * price   without any cost or fees
        public decimal GetTotal() => SubTotal + DeliveryMethod.Price; // readonly property and it's name is Total Get=>()
    }
}
