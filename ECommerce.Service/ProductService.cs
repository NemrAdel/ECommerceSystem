using AutoMapper;
using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Service.Specifications.ProductSpecifications;
using ECommerce.Services.Abstraction;
using ECommerce.Shared;
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
        public async Task<PaginatedResult<ProductDTO>> GetAllProductAsync(ProductQueryParams queryParams)
        {
            var repo= _unitOfWork.GetRepository<Product,int>();
            // get all products with their types and brands
            var spec=new ProductWithTypeAndBrandSpec(queryParams);
            var products = await repo.GetAllAsync(spec);
            var DataToReturn = _mapper.Map<IEnumerable<ProductDTO>>(products);
            var CountOfReturnDate = DataToReturn.Count();
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex,CountOfReturnDate,CountOfReturnDate,DataToReturn);
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandAsync()
        {
            var Brands = await _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);
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
