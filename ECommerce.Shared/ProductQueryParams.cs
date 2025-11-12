using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared
{
    public class ProductQueryParams
    {
        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public string? search { get; set; }

        public ProductSortingOptions? sort { get; set; }
    }
}
