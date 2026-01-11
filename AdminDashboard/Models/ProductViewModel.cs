using ECommerce.Doamin.Entities.ProductModule;
using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Product Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description  is required")]
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? PictureUrl { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1,10000)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "BrandId  is required")]
        public int BrandId { get; set; }
        public ProductBrand Brand { get; set; }

        [Required(ErrorMessage = "TypeId is required")]
        public int TypeId { get; set; }
        public ProductType Type { get; set; }
    }
}
