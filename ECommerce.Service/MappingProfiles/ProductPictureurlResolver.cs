using AutoMapper;
using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Shared.DTOs.ProductDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.MappingProfiles
{
    public class ProductPictureurlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureurlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;
            if(source.PictureUrl.StartsWith("http")|| source.PictureUrl.StartsWith("https"))
                return source.PictureUrl;
            var BaseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            var pictureUrl = $"{BaseUrl}{source.PictureUrl}";
            return pictureUrl;
        }
    }
}
