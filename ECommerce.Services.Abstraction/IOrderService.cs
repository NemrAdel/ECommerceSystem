using ECommerce.Shared.DTOs.OderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IOrderService
    {
        Task<OrderToReturnDTO> CreateOrderAsync(OrderDTO orderDTO, string Email);
    }
}
