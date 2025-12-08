using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.OderDTOs
{
    public class OrderToReturnDTO
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; }
        public OrderAddressDTO Address { get; set; }
        public string DeliveryMethod { get; set; }
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }

        

        
    }
}
