using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Basket.Authorization;
using MultiShop.Basket.Dtos;
using MultiShop.Basket.Services;
using MultiShop.Basket.Services.CurrentUser;

namespace MultiShop.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ICurrentUserService _currentUserService;

        public BasketsController(IBasketService basketService, ICurrentUserService currentUserService)
        {
            _basketService = basketService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketAsync(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();

            var basket = await _basketService.GetBasketAsync(userId, cancellationToken);

            if (basket == null)
                return NotFound();

            return Ok(basket);
        }

        [HttpPut]
        public async Task<IActionResult> SaveBasketAsync(BasketTotalDto basketTotalDto, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            basketTotalDto.UserId = userId;
            var result = await _basketService.SaveBasketAsync(basketTotalDto, cancellationToken);
            if(!result)
                return StatusCode(StatusCodes.Status503ServiceUnavailable);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasketAsync(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _basketService.DeleteBasketAsync(userId, cancellationToken);
            if(!result)
                return NotFound();

            return NoContent();
        }
    }
}
