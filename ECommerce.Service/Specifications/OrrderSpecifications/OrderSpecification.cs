using ECommerce.Doamin.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications.OrrderSpecifications
{
    public class OrderSpecification:BaseSpecifications<Order,Guid>
    {
        public OrderSpecification(string email):base(o=>o.UserEmail==email)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);
            AddOrderByDesc(x => x.OrderDate);
        }
        public OrderSpecification(Guid id, string email):base(o=>o.UserEmail==email && o.Id==id)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);
        }
    }
}
