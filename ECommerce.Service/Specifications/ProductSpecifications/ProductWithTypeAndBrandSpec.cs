using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Specifications.ProductSpecifications
{
    internal class ProductWithTypeAndBrandSpec:BaseSpecifications<Product,int>
    {
        //1- brandId is not null && p=>p.BrandID=brandId
        //2- typeId is not null && p=>p.typeID=typeId
        //3- the two together
        public ProductWithTypeAndBrandSpec(ProductQueryParams queryParams)
    :   base( p =>
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
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
        public ProductWithTypeAndBrandSpec(int id):base(x=>x.Id==id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
