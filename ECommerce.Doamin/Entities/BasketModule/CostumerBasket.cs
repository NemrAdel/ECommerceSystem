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

        public ICollection<BasketItem> Items { get; set; } =[];
    }
}
