using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Entities.OrderModule
{
    public enum OrderStatus
    {
        Pending,
        PaymentFailed,
        PaymentReceived
    }
}
