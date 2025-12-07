using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class PaymentService : IPaymentService
    {
        public Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            
        }
    }
}
