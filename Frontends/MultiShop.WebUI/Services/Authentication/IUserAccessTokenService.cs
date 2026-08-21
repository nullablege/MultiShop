namespace MultiShop.WebUI.Services.Authentication
{
    public interface IUserAccessTokenService
    {
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    }
}
