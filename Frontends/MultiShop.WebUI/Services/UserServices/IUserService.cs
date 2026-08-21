using MultiShop.WebUI.Models.Identity;

namespace MultiShop.WebUI.Services.UserServices
{
    public interface IUserService
    {
        Task<UserDetailViewModel> GetUserInfoAsync(CancellationToken cancellationToken = default);
    }
}
