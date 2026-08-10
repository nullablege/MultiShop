using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.DTOs.BrandDTOs;
using MultiShop.Catalog.Services.BrandServices;

namespace MultiShop.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = CatalogAuthorizationConstants.Policy)]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandsController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResultBrandDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _brandService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdBrandDto>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var brand = await _brandService.GetByIdAsync(id, cancellationToken);
        if (brand == null)
            return NotFound();

        return Ok(brand);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateBrandDto createBrandDto, CancellationToken cancellationToken = default)
    {
        await _brandService.CreateAsync(createBrandDto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateBrandDto updateBrandDto, CancellationToken cancellationToken = default)
    {
        if (id != updateBrandDto.BrandId)
            return BadRequest();

        if (!await _brandService.UpdateAsync(updateBrandDto, cancellationToken))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        if (!await _brandService.DeleteAsync(id, cancellationToken))
            return NotFound();

        return NoContent();
    }
}
