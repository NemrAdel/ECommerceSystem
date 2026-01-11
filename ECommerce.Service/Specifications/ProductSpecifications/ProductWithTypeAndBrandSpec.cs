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
    public class ProductWithTypeAndBrandSpec:BaseSpecifications<Product,int>
    {
        //1- brandId is not null && p=>p.BrandID=brandId
        //2- typeId is not null && p=>p.typeID=typeId
        //3- the two together
        public ProductWithTypeAndBrandSpec(ProductQueryParams queryParams , bool forDashboard = false)
            :base( ProductSpecificationsHelper.GetCriteria(queryParams))
        
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            switch (queryParams.sort)
            {
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(p => p.Price);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(p => p.Name);
                    break;
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
            if(!forDashboard)
                ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
        public ProductWithTypeAndBrandSpec(int id):base(x=>x.Id==id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
