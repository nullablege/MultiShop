using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiShop.Identity.Models;

namespace MultiShop.Identity.Data
{
    public class MultiShopIdentityDbContext:IdentityDbContext<AppUser>
    {
        public MultiShopIdentityDbContext(DbContextOptions<MultiShopIdentityDbContext> options) : base(options)
        {
        }
    }
}
