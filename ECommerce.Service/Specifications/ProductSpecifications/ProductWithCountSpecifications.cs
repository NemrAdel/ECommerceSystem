using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications.ProductSpecifications
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecifications(ProductQueryParams queryParams)
            : base(ProductSpecificationsHelper.GetCriteria(queryParams))
        {

        }
    }
}
