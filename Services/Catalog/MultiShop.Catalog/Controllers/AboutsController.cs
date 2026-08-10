using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.DTOs.AboutDTOs;
using MultiShop.Catalog.Services.AboutServices;

namespace MultiShop.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = CatalogAuthorizationConstants.Policy)]
public class AboutsController : ControllerBase
{
    private readonly IAboutService _aboutService;

    public AboutsController(IAboutService aboutService)
    {
        _aboutService = aboutService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResultAboutDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _aboutService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdAboutDto>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var about = await _aboutService.GetByIdAsync(id, cancellationToken);
        if (about == null)
            return NotFound();

        return Ok(about);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateAboutDto createAboutDto, CancellationToken cancellationToken = default)
    {
        await _aboutService.CreateAsync(createAboutDto, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateAboutDto updateAboutDto, CancellationToken cancellationToken = default)
    {
        if (id != updateAboutDto.AboutId)
            return BadRequest();

        if (!await _aboutService.UpdateAsync(updateAboutDto, cancellationToken))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        if (!await _aboutService.DeleteAsync(id, cancellationToken))
            return NotFound();

        return NoContent();
    }
}
