using Microsoft.AspNetCore.Identity;

namespace E_commerceAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
    }
}
