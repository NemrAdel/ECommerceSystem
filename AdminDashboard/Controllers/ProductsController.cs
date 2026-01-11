using ECommerce.Doamin.Contracts;
using ECommerce.Doamin.Entities.ProductModule;
using ECommerce.Presistence.Data.DbContexts;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Service.Specifications.ProductSpecifications;
using ECommerce.Shared;
using System.Threading.Tasks;
using AdminDashboard.Models;
using AdminDashboard.Helpers;

namespace AdminDashboard.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var queryPatams = new ProductQueryParams();
            var specification = new ProductWithTypeAndBrandSpec(queryPatams, true);

            var products = await productRepo.GetAllAsync(specification);
            var productsViewModel = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                PictureUrl = p.PictureUrl,
                Price = p.Price,
                BrandId = p.ProductBrandId,
                TypeId = p.ProductTypeId,
                Brand = p.ProductBrand,
                Type = p.ProductType
            });
            return View(productsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Image is not null)
                {
                    model.PictureUrl = PictureSettings.UploadFile(model.Image,"products");
                }
                var mappedProduct = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    PictureUrl = model.PictureUrl!,
                    Price = model.Price,
                    ProductBrandId = model.BrandId,
                    ProductTypeId = model.TypeId
                };
                var productRepo = _unitOfWork.GetRepository<Product, int>();
                await productRepo.AddAsync(mappedProduct);
                await _unitOfWork.saveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
