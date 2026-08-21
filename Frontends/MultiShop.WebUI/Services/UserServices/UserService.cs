using MultiShop.WebUI.Models.Identity;

namespace MultiShop.WebUI.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDetailViewModel> GetUserInfoAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<UserDetailViewModel>("api/users/me", cancellationToken);

            if (result == null)
                throw new InvalidOperationException("Kullanıcı bilgisi alınamadı.");

            return result;
        }
    }
}
