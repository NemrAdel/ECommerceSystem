using ECommerce.Services.Abstraction;
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
        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProduct(int? brandId,int? typeId)
        {
            var products = await  _productService.GetAllProductAsync(brandId,typeId);
            return Ok(products);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
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
