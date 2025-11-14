using AutoMapper;
using ECommerce.Doamin.Entities.BasketModule;
using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingProfiles
{
    public class BasketProfile:Profile
    {
        public BasketProfile()
        {

            CreateMap<BasketDTO, CostumerBasket>().ReverseMap();
            CreateMap<BasketItemDTO, BasketItem>().ReverseMap();
        }
    }
}
