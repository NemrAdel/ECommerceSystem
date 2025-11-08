using AutoMapper;
using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingProfiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDTO>();


            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductBrand, opt =>
                opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt =>
                opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, opt =>
                opt.MapFrom<ProductPictureurlResolver>());

            CreateMap<ProductType, TypeDTO>();
        }

    }
}
