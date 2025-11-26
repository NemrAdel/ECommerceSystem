using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Entities.OrderModule
{
    public class OrderAddress
    {
        public string? FirsName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Country { get; set; }
    }
}
