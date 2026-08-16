namespace MultiShop.WebUI.Services.Authentication;

public interface ICatalogAccessTokenService
{
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}
