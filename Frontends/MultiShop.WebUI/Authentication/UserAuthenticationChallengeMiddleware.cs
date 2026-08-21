using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace MultiShop.WebUI.Authentication
{
    public class UserAuthenticationChallengeMiddleware
    {
        private readonly RequestDelegate _next;

        public UserAuthenticationChallengeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UserAuthenticationRequiredException) when (!context.Response.HasStarted)
            {
                var returnUrl = $"{context.Request.PathBase}{context.Request.Path}{context.Request.QueryString}";

                context.Response.Clear();
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = returnUrl } );
            }
        }
    }
}
