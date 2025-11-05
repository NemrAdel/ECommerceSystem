using AutoMapper;
using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Service.Specifications.ProductSpecifications;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandAsync()
        {
            var Brands = await _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductAsync(int? brandId,int? typeId)
        {
            // get all products with their types and brands
            var spec=new ProductWithTypeAndBrandSpec(brandId,typeId);
            var products = await _unitOfWork.GetRepository<Product,int>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypeAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(types);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var spec=new ProductWithTypeAndBrandSpec(id);
            var product =await  _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec);
            return _mapper.Map<ProductDTO>(product);
        }
    }
}
