using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Entities.BasketModule
{
    public class CostumerBasket
    {
        public string Id { get; set; } = default!; // Created from frontEnd [GUID]

        public int? DelviryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
        public string? PaymentIntentID { get; set; }
        public string? ClientSecret { get; set; }

        public ICollection<BasketItem> Items { get; set; } =[];
    }
}
