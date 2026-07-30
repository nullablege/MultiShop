using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MultiShop.Identity.Models;
using OpenIddict.Abstractions;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MultiShop.Identity.Services
{
    public sealed class OpenIddictPrincipalService : IOpenIddictPrincipalService
    {
        private readonly UserManager<AppUser> _userManager;

        public OpenIddictPrincipalService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> CreateAsync(AppUser user, IEnumerable<string> scopes, CancellationToken cancellationToken = default )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);
            var id = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            if (email == null)
                throw new InvalidOperationException("Kullanıcının e-posta bilgisi bulunamadı");
            var userName = await _userManager.GetUserNameAsync(user);
            if (userName == null)
                throw new InvalidOperationException("Kullanıcı adı bulunamadı");
            var roles = await _userManager.GetRolesAsync(user);

            var displayName = $"{user.Name} {user.Surname}".Trim();

            if (string.IsNullOrWhiteSpace(displayName))
                displayName = userName;


            identity.AddClaims(roles.Select(role => new Claim(Claims.Role, role)));
            identity.SetClaim(Claims.Name, displayName);
            identity.SetClaim(Claims.Email, email);
            identity.SetClaim(Claims.Subject, id);
            identity.SetClaim(Claims.PreferredUsername, userName);

            var requestedScopes = scopes.ToArray();

            identity.SetScopes(requestedScopes);

            identity.SetResources(requestedScopes.Where(scope => scope.EndsWith("_api", StringComparison.Ordinal)));

            identity.SetDestinations(GetDestinations);

            cancellationToken.ThrowIfCancellationRequested();

            return new ClaimsPrincipal(identity);

        }

        private static IEnumerable<string> GetDestinations(Claim claim)
        {

            var destinations = new List<string>
            {
                Destinations.AccessToken
            };

            if ((claim.Type == Claims.PreferredUsername || claim.Type == Claims.Name) && claim.Subject!.HasScope(Scopes.Profile))
                    destinations.Add(Destinations.IdentityToken);


            if (claim.Type == Claims.Subject)
                destinations.Add(Destinations.IdentityToken);


            if (claim.Type == Claims.Role && claim.Subject!.HasScope(Scopes.Roles))
                destinations.Add(Destinations.IdentityToken);

            if(claim.Type == Claims.Email && claim.Subject!.HasScope(Scopes.Email))
                destinations.Add(Destinations.IdentityToken);

            return destinations;
        }
    }
}
