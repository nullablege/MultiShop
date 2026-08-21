using MultiShop.WebUI.Models.Identity;

namespace MultiShop.WebUI.Services.UserServices;

public interface IAdminUserService
{
    Task<IReadOnlyList<AdminUserListItemDto>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
