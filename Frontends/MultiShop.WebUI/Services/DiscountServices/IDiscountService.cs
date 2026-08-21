using MultiShop.WebUI.Models.Discount;

namespace MultiShop.WebUI.Services.DiscountServices;

public interface IDiscountService
{
    Task<DiscountCouponDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
