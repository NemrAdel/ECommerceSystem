using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications.ProductSpecifications
{
    public class ProductWithCountSpecifications(ProductQueryParams productQueryParams):base<>( p =>
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
        )

        )
    {

    }
}
