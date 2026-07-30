using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Identity.Models;
using MultiShop.Identity.Services;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace MultiShop.Identity.Controllers
{

    public class AuthorizationController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOpenIddictPrincipalService _openIddictPrincipalService;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthorizationController(UserManager<AppUser> userManager, IOpenIddictPrincipalService openIddictPrincipalService, SignInManager<AppUser> signInManager)
        {
            _openIddictPrincipalService = openIddictPrincipalService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private IActionResult InvalidGrant(string description)
        {
            return Forbid(
                authenticationSchemes:
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(
                    new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error]
                            = OpenIddictConstants.Errors.InvalidGrant,

                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription]
                            = description
                    }));
        }


        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize(CancellationToken cancellationToken = default)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("OpenID Connect isteği alınamadı");

            var authenticationResult = await HttpContext.AuthenticateAsync();

            if (!authenticationResult.Succeeded)
            {
                var redirectUri = Request.PathBase + Request.Path + QueryString.Create(Request.HasFormContentType ? Request.Form : Request.Query);

                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = redirectUri
                });

            }

            var user = await _userManager.GetUserAsync(authenticationResult.Principal);
            if (user == null)
                throw new InvalidOperationException("Oturum açmış kullanıcının bilgileri bulunamadı");

            var principal = await _openIddictPrincipalService.CreateAsync(user, request.GetScopes(), cancellationToken);

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpPost("~/connect/token")]
        [IgnoreAntiforgeryToken]
        [Produces("application/json")]
        public async Task<IActionResult> Exchange(CancellationToken cancellationToken = default)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("OpenID Connect isteği alınamadı");

            if (!request.IsAuthorizationCodeGrantType())
                throw new InvalidOperationException("Grant Type Hata");


            var authenticationResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var codePrincipal = authenticationResult.Principal;
            if (!authenticationResult.Succeeded || codePrincipal == null)
                return InvalidGrant("Authorization code doğrulanamadı");


            var userId = authenticationResult.Principal?.GetClaim(OpenIddictConstants.Claims.Subject);
            if (string.IsNullOrWhiteSpace(userId))
                return InvalidGrant("Authorization code kullanıcı bilgisi içermiyor");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return InvalidGrant("Authorization code'a bağlı kullanıcı bulunamadı");

            if (!await _signInManager.CanSignInAsync(user))
                return InvalidGrant("Kullanıcının girişine izin verilmiyor");


            var principal = await _openIddictPrincipalService.CreateAsync(user, codePrincipal.GetScopes(), cancellationToken);
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        }


        [HttpGet("~/connect/logout")]
        [HttpPost("~/connect/logout")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return SignOut(authenticationSchemes:
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, properties: new AuthenticationProperties
                    {
                        RedirectUri = "/"
                    });
        }

    }
}
