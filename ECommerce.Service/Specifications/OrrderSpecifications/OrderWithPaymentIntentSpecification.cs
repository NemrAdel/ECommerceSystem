using ECommerce.Doamin.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications.OrrderSpecifications
{
    public class OrderWithPaymentIntentSpecification:BaseSpecifications<Order,Guid>
    {
        public OrderWithPaymentIntentSpecification(string PaymentIntendId) : base(o => o.PaymentIntentId == PaymentIntendId)
        {
        }
    }
}
