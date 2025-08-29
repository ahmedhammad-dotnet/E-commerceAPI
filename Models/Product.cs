using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations;

namespace E_commerceAPI.Models
{
    public class Product
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
        public string ImgUrl { get; set; }
        [Range(0, 5)]
        public double Rate { get; set; }
        [Required]
        public double Quantity { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        
        [Required]
        public int CategoryId { get; set; }
        
       
    }
}
