using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.OderDTOs
{
    public class OrderDTO
    {
        public string BasketId { get; set; } // init like record after first set i can't modify it
        public int DeliveryMethodId { get; set; }
        public OrderAddressDTO Address { get; set; }
    }
}
