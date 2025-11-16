using ECommerce.Presentation.Attributes;
using ECommerce.Services.Abstraction;
using ECommerce.Shared;
using ECommerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController:ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [RedisCache]
        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProduct([FromQuery]ProductQueryParams queryParams)//clean code : must be by max 3 parameters in the function so should make object parameter design pattern
        {
            var products = await  _productService.GetAllProductAsync(queryParams);
            return Ok(products);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
                var product = await _productService.GetProductByIdAsync(id);
                if (product is null)
                    return NotFound("Product Not Found");
                return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult <IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var brands = await _productService.GetAllBrandAsync();
            return Ok(brands);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var types = await _productService.GetAllTypeAsync();
            return Ok(types);
        }
    }
}
