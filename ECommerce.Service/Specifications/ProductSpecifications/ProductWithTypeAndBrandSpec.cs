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
        //1- brandId is not null && p=>p.BrandID=brandId
        //2- typeId is not null && p=>p.typeID=typeId
        //3- the two together
        public ProductWithTypeAndBrandSpec(int? brandId,int? typeId):base(p=>((!brandId.HasValue)||(p.ProductBrandId==brandId.Value))
        &&((!typeId.HasValue)||(p.ProductTypeId==typeId.Value)))
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
