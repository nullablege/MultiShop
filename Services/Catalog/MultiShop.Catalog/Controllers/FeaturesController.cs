using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.DTOs.FeatureDTOs;
using MultiShop.Catalog.Services.FeatureServices;

namespace MultiShop.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = CatalogAuthorizationConstants.Policy)]
public class FeaturesController : ControllerBase
{
    private readonly IFeatureService _featureService;

    public FeaturesController(IFeatureService featureService)
    {
        _featureService = featureService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResultFeatureDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _featureService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdFeatureDto>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var feature = await _featureService.GetByIdAsync(id, cancellationToken);
        if (feature == null)
            return NotFound();

        return Ok(feature);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateFeatureDto createFeatureDto, CancellationToken cancellationToken = default)
    {
        await _featureService.CreateAsync(createFeatureDto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateFeatureDto updateFeatureDto, CancellationToken cancellationToken = default)
    {
        if (id != updateFeatureDto.FeatureId)
            return BadRequest();

        if (!await _featureService.UpdateAsync(updateFeatureDto, cancellationToken))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        if (!await _featureService.DeleteAsync(id, cancellationToken))
            return NotFound();

        return NoContent();
    }
}
