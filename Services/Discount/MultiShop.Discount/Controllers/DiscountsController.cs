using Microsoft.AspNetCore.Mvc;
using MultiShop.Discount.Dtos;
using MultiShop.Discount.Services;

namespace MultiShop.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ResultDiscountCouponDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var values = await _discountService.GetAllDiscountCouponAsync(cancellationToken);
            return Ok(values);
        }

        [HttpGet("{couponId:int}")]
        public async Task<ActionResult<GetByIdDiscountCouponDto>> GetById(int couponId, CancellationToken cancellationToken = default)
        {
            var value = await _discountService.GetByIdDiscountCouponAsync(couponId, cancellationToken);
            if (value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<ResultDiscountCouponDto>> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            var value = await _discountService.GetCodeDetailByCodeAsync(code, cancellationToken);
            if (value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateDiscountCouponDto createDiscountCouponDto, CancellationToken cancellationToken = default)
        {
            await _discountService.CreateDiscountCouponAsync(createDiscountCouponDto, cancellationToken);
            return NoContent();
        }

        [HttpPut("{couponId:int}")]
        public async Task<ActionResult> Update(int couponId, UpdateDiscountCouponDto updateDiscountCouponDto, CancellationToken cancellationToken = default)
        {
            if (couponId != updateDiscountCouponDto.CouponId)
            {
                return BadRequest();
            }

            var result = await _discountService.UpdateDiscountCouponAsync(updateDiscountCouponDto, cancellationToken);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{couponId:int}")]
        public async Task<ActionResult> Delete(int couponId, CancellationToken cancellationToken = default)
        {
            var result = await _discountService.DeleteDiscountCouponAsync(couponId, cancellationToken);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount(CancellationToken cancellationToken = default)
        {
            var value = await _discountService.GetDiscountCouponCountAsync(cancellationToken);
            return Ok(value);
        }

        [HttpGet("code/{code}/rate")]
        public async Task<ActionResult<int>> GetRateByCode(string code, CancellationToken cancellationToken = default)
        {
            var value = await _discountService.GetDiscountCouponRateByCodeAsync(code, cancellationToken);
            if (value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }
    }
}
