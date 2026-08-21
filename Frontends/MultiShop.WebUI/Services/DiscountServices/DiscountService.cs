using System.Net;
using MultiShop.WebUI.Models.Discount;

namespace MultiShop.WebUI.Services.DiscountServices;

public sealed class DiscountService : IDiscountService
{
    private readonly HttpClient _httpClient;

    public DiscountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DiscountCouponDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return null;
        }

        var normalizedCode = code.Trim();
        var response = await _httpClient.GetAsync($"discounts/code/{Uri.EscapeDataString(normalizedCode)}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DiscountCouponDto>(cancellationToken);
    }
}
