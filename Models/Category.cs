using System.ComponentModel.DataAnnotations;

namespace E_commerceAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Minimun length 3")]
        [MaxLength(25, ErrorMessage = "Minimun length 25")]
        public string Name { get; set; }

        public ICollection<Product> products { get; } = new List<Product>();
    }
}
