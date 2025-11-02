using ECommerce.Doamin.Entities.ProductModule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id}/{Name}")]
        public ActionResult<Product> GrtById(int id,string name)
        {
            return new Product
            {
                Id=id,
                Name= name
            };
            
        }
    }
}
