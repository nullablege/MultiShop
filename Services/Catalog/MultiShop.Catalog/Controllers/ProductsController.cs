using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.DTOs.ProductDTOs;
using MultiShop.Catalog.Services.ProductServices;

namespace MultiShop.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = CatalogAuthorizationConstants.Policy)]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet("by-category/{categoryId}")]
    public async Task<ActionResult<IReadOnlyList<ResultProductDto>>> GetProductsByCategoryId(string categoryId, CancellationToken cancellationToken = default)
    {
        var values = await _productService.GetProductsByCategoryAsync(categoryId, cancellationToken);
        return Ok(values);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResultProductDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var values = await _productService.GetAllAsync(cancellationToken);
        return Ok(values);
    }

    [HttpGet("with-category")]
    public async Task<ActionResult<IReadOnlyList<ResultProductWithCategoryDto>>> GetWithCategory(CancellationToken cancellationToken = default)
    {
        var values = await _productService.GetWithCategoryAsync(cancellationToken);
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdProductDto>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var value = await _productService.GetByIdAsync(id, cancellationToken);
        if(value == null)
            return NotFound();

        return Ok(value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _productService.DeleteAsync(id, cancellationToken);
        if (result)
            return NoContent();

        return NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, UpdateProductDto updateProductDto, CancellationToken cancellationToken = default)
    {
        if(id != updateProductDto.ProductId)
            return BadRequest();

        var result = await _productService.UpdateAsync(updateProductDto, cancellationToken);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateProductDto createProductDto, CancellationToken cancellationToken = default)
    {
        await _productService.CreateAsync(createProductDto, cancellationToken);
        return NoContent();
    }
}
