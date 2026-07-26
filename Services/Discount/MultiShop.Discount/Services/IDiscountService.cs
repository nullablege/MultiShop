using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services
{
    public interface IDiscountService
    {
        Task<IReadOnlyList<ResultDiscountCouponDto>> GetAllDiscountCouponAsync(CancellationToken cancellationToken = default);
        Task<GetByIdDiscountCouponDto?> GetByIdDiscountCouponAsync(int couponId, CancellationToken cancellationToken = default);
        Task<ResultDiscountCouponDto?> GetCodeDetailByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task CreateDiscountCouponAsync(CreateDiscountCouponDto createDiscountCouponDto, CancellationToken cancellationToken = default);
        Task<bool> UpdateDiscountCouponAsync(UpdateDiscountCouponDto updateDiscountCouponDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteDiscountCouponAsync(int couponId, CancellationToken cancellationToken = default);
        Task<int> GetDiscountCouponCountAsync(CancellationToken cancellationToken = default);
        Task<int?> GetDiscountCouponRateByCodeAsync(string code, CancellationToken cancellationToken = default);
    }
}
