using ECommerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class PaymentController:ApiBaseController
    {
        [HttpPost("{BasketId}")]
        public ActionResult<BasketDTO> CreateOrUpdatePaymentIntent(string BasketId)
        {
            return Ok();

        }
    }
}
