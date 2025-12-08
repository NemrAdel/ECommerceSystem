using AutoMapper;
using ECommerce.Doamin.Entities.IdentityModule;
using ECommerce.Shared.DTOs.SecurityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingProfiles
{
    public class Authentication_Profile:Profile
    {
        public Authentication_Profile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
