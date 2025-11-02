using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Doamin.Entities.ProductModule
{
    public class Product: BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; } = default!;


        // Navigation Properties

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;
        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; } = null!;


    }
}
