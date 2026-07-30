using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Identity.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace MultiShop.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(
        AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
        Policy = IdentityAuthorizationConstants.IdentityApiPolicy)]
    public class IdentityAccessController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Access token geçerli");
        }
    }
}
