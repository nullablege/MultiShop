using Microsoft.AspNetCore.Identity;

namespace MultiShop.Identity.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }
}
