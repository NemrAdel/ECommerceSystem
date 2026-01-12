using ECommerce.Doamin.Entities.OrderModule;

namespace ECommerce.Service.Specifications.OrrderSpecifications
{
    public class OrderWithPaymentIntentSpecification:BaseSpecifications<Order,Guid>
    {
        public OrderWithPaymentIntentSpecification(string PaymentIntendId) : base(o => o.PaymentIntentId == PaymentIntendId)
        {
            AddInclude(o => o.Items);
        }
    }
}
