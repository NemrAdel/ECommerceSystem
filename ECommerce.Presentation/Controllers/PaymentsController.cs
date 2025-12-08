using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ECommerce.Presentation.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("{BasketId}")]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var Result=await _paymentService.CreateOrUpdatePaymentIntentAsync(BasketId);
            return HandleResult(Result);
        }

        [HttpPost("weebhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature=Request.Headers["Stripe-Signature"];
            await _paymentService.UpdateOrderPaymentStatus(json, stripeSignature!);
            return new EmptyResult();
        }
        
    }
}
