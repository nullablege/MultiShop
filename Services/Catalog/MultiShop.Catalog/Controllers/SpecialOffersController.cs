using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.DTOs.SpecialOfferDTOs;
using MultiShop.Catalog.Services.SpecialOfferServices;

namespace MultiShop.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = CatalogAuthorizationConstants.Policy)]
public class SpecialOffersController : ControllerBase
{
    private readonly ISpecialOfferService _specialOfferService;

    public SpecialOffersController(ISpecialOfferService specialOfferService)
    {
        _specialOfferService = specialOfferService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResultSpecialOfferDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var specialOffers = await _specialOfferService.GetAllAsync(cancellationToken);
        return Ok(specialOffers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdSpecialOfferDto>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var specialOffer = await _specialOfferService.GetByIdAsync(id, cancellationToken);
        if (specialOffer == null)
            return NotFound();

        return Ok(specialOffer);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateSpecialOfferDto createSpecialOfferDto, CancellationToken cancellationToken = default)
    {
        await _specialOfferService.CreateAsync(createSpecialOfferDto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateSpecialOfferDto updateSpecialOfferDto, CancellationToken cancellationToken = default)
    {
        if (id != updateSpecialOfferDto.SpecialOfferId)
            return BadRequest();

        var result = await _specialOfferService.UpdateAsync(updateSpecialOfferDto, cancellationToken);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _specialOfferService.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
