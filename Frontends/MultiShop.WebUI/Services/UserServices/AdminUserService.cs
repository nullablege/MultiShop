using MultiShop.WebUI.Models.Identity;

namespace MultiShop.WebUI.Services.UserServices;

public sealed class AdminUserService : IAdminUserService
{
    private readonly HttpClient _httpClient;

    public AdminUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<AdminUserListItemDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var users = await _httpClient.GetFromJsonAsync<List<AdminUserListItemDto>>(
            "api/users",
            cancellationToken);

        return users is null ? Array.Empty<AdminUserListItemDto>() : users;
    }
}
