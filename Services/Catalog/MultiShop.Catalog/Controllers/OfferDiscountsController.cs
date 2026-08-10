using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.DTOs.OfferDiscountDTOs;
using MultiShop.Catalog.Services.OfferDiscountServices;

namespace MultiShop.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = CatalogAuthorizationConstants.Policy)]
public class OfferDiscountsController : ControllerBase
{
    private readonly IOfferDiscountService _offerDiscountService;

    public OfferDiscountsController(IOfferDiscountService offerDiscountService)
    {
        _offerDiscountService = offerDiscountService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResultOfferDiscountDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _offerDiscountService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdOfferDiscountDto>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var offerDiscount = await _offerDiscountService.GetByIdAsync(id, cancellationToken);
        if (offerDiscount == null)
            return NotFound();

        return Ok(offerDiscount);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateOfferDiscountDto createOfferDiscountDto, CancellationToken cancellationToken = default)
    {
        await _offerDiscountService.CreateAsync(createOfferDiscountDto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateOfferDiscountDto updateOfferDiscountDto, CancellationToken cancellationToken = default)
    {
        if (id != updateOfferDiscountDto.OfferDiscountId)
            return BadRequest();

        if (!await _offerDiscountService.UpdateAsync(updateOfferDiscountDto, cancellationToken))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        if (!await _offerDiscountService.DeleteAsync(id, cancellationToken))
            return NotFound();

        return NoContent();
    }
}
