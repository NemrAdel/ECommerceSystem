using AutoMapper;
using ECommerce.Doamin.Entities.OrderModule;
using ECommerce.Shared.DTOs.OderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingProfiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAddressDTO, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(dest => dest.DeliveryMethod, opt =>
                opt.MapFrom(src => src.DeliveryMethod.ShortName));
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt =>
                opt.MapFrom(src => src.Product.ProductName))
                .ForMember(d => d.PictureUrl, o =>
                o.MapFrom<OrderItemPictureResolver>());
            CreateMap<DeliveryMethod, DeliveryMethodDTO>();    

        }
    }
}
