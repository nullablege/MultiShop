using MultiShop.Identity.Models;
using System.Security.Claims;

namespace MultiShop.Identity.Services
{
    public interface IOpenIddictPrincipalService
    {
        Task<ClaimsPrincipal> CreateAsync(AppUser user, IEnumerable<string> scopes, CancellationToken cancellationToken = default);
    }
}
