using ECommerce.Shared.CommonRespones;
using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IPaymentService
    {
        Task<Result<BasketDTO>> CreateOrUpdatePaymentIntentAsync(string BasketId);
    }
}
