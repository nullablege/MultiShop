using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.DTOs.FeatureSliderDTOs;
using MultiShop.Catalog.Services.FeatureSliderServices;

namespace MultiShop.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = CatalogAuthorizationConstants.Policy)]
public class FeatureSlidersController : ControllerBase
{
    private readonly IFeatureSliderService _featureSliderService;

    public FeatureSlidersController(IFeatureSliderService featureSliderService)
    {
        _featureSliderService = featureSliderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResultFeatureSliderDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var values = await _featureSliderService.GetAllAsync(cancellationToken);
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdFeatureSliderDto>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var value = await _featureSliderService.GetByIdAsync(id, cancellationToken);
        if (value == null)
            return NotFound();

        return Ok(value);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateFeatureSliderDto createFeatureSliderDto, CancellationToken cancellationToken = default)
    {
        await _featureSliderService.CreateAsync(createFeatureSliderDto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateFeatureSliderDto updateFeatureSliderDto, CancellationToken cancellationToken = default)
    {
        if (id != updateFeatureSliderDto.FeatureSliderId)
            return BadRequest();

        var result = await _featureSliderService.UpdateAsync(updateFeatureSliderDto, cancellationToken);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _featureSliderService.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
