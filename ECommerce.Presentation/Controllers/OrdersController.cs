using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs.OderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ECommerce.Presentation.Controllers
{
    public class OrdersController:ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _orderService.CreateOrderAsync(orderDTO, email!);
            return HandleResult(result);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _orderService.GetAllOrdersAsync(email!);
            return HandleResult(result);
        }
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _orderService.GetOrderByIdAsync(id,email!);
            return HandleResult(result);
        }
        [AllowAnonymous] // any one can reach it 
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethod()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _orderService.GetAllDeliveryMethodAsync();
            return HandleResult(result);
        }
        
    }
}
