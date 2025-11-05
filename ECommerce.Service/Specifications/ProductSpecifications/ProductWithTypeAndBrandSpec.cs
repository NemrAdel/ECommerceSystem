using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications.ProductSpecifications
{
    internal class ProductWithTypeAndBrandSpec:BaseSpecifications<Product,int>
    {
        public ProductWithTypeAndBrandSpec():base()
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
