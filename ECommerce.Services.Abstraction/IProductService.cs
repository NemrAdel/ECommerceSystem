using ECommerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductAsync(int? brandId,int? typeId);
        Task <ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDTO>> GetAllBrandAsync();
        Task<IEnumerable<TypeDTO>> GetAllTypeAsync();
    }
}
