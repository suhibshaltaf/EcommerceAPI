
using System.ComponentModel.DataAnnotations;


namespace Ecommerce.Core.Entities.DTO
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int Category_Id { get; set; }
    }
}
