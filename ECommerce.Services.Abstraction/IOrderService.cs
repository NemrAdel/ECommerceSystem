using ECommerce.Shared.CommonRespones;
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
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email);

        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodAsync();
        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email);

        Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id,string email);
     }
}
