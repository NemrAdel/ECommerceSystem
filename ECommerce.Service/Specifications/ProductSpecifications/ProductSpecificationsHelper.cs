using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications.ProductSpecifications
{
    public class ProductSpecificationsHelper
    {
        public static Expression<Func<Product,bool>> GetCriteria(ProductQueryParams queryParams)
        {
            return p =>
        (
            (!queryParams.brandId.HasValue) || (p.ProductBrandId == queryParams.brandId.Value)
        )
        &&
        (
            (!queryParams.typeId.HasValue) || (p.ProductTypeId == queryParams.typeId.Value)
        )
        &&
        (
            (string.IsNullOrEmpty(queryParams.search)) || (p.Name.ToLower().Contains(queryParams.search.ToLower()))
        );

            
        }
    }
}
