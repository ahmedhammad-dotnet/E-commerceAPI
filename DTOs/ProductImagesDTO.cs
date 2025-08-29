using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_commerceAPI.DTOs
{
    public class ProductImagesDTO
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "mininem 3")]
        [MaxLength(25, ErrorMessage = "mininem 25")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(0, 100000)]
        public decimal Price { get; set; }
        [ValidateNever]
        public IFormFile ImgUrl { get; set; }
        [Range(0, 5)]
        public double Rate { get; set; }
        [Range(1,10000)]
        public double Quantity { get; set; }
        [Required]
        public int CategoryId { get; set; }

    }
}
