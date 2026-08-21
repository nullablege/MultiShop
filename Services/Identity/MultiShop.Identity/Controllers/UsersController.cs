using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Identity.Authorization;
using MultiShop.Identity.DTOs;
using MultiShop.Identity.Models;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace MultiShop.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(
        AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
        Policy = IdentityAuthorizationConstants.IdentityApiPolicy)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public UsersController(UserManager<AppUser> userManager) {
            _userManager = userManager;
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserProfileDto>> GetMe()
        {
            var userId = User.FindFirst(OpenIddictConstants.Claims.Subject);

            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId.Value);

            if (user == null)
                return Unauthorized();

            var dto = new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Name = user.Name,
                Surname = user.Surname
            };
            return Ok(dto);

        }

        [HttpGet]
        [Authorize(
            AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
            Policy = IdentityAuthorizationConstants.ManagementPolicy)]
        public async Task<ActionResult<IReadOnlyList<AdminUserListItemDto>>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .AsNoTracking()
                .OrderBy(user => user.UserName)
                .Select(user => new AdminUserListItemDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Name = user.Name,
                    Surname = user.Surname
                })
                .ToListAsync(cancellationToken);

            return Ok(users);
        }

        [HttpGet("count")]
        [Authorize(
            AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
            Policy = IdentityAuthorizationConstants.ManagementPolicy)]
        public async Task<ActionResult<UserCountDto>> GetCountAsync(
            CancellationToken cancellationToken)
        {
            var count = await _userManager.Users.CountAsync(cancellationToken);
            return Ok(new UserCountDto { Count = count });
        }

    }
}
