using AutoMapper;
using ECommerce.Doamin.Entities.OrderModule;
using ECommerce.Shared.DTOs.OderDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingProfiles
{
    public class OrderItemPictureResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.Product.PictureUrl))
                return string.Empty;
            if(source.Product.PictureUrl.StartsWith("http")|| source.Product.PictureUrl.StartsWith("https"))
                return source.Product.PictureUrl;
            var baseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            if(string .IsNullOrEmpty(baseUrl))
                return string.Empty ;
            return $"{baseUrl}{source.Product.PictureUrl}";
        }
    }
}
