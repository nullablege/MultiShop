using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.DTOs.CategoryDTOs;
using MultiShop.Catalog.Services.CategoryServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = CatalogAuthorizationConstants.Policy)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ResultCategoryDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var value = await _categoryService.GetAllAsync(cancellationToken);
            return Ok(value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetByIdCategoryDto>> GetByIdCategoryAsync(string id, CancellationToken cancellationToken = default)
        {
            var value = await _categoryService.GetByIdAsync(id, cancellationToken);
            if (value == null)
                return NotFound();

            return Ok(value);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken = default)
        {
            await _categoryService.CreateAsync(createCategoryDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(string id, CancellationToken cancellationToken = default)
        {
            var result =  await _categoryService.DeleteAsync(id, cancellationToken);
            if(result)
                return NoContent();

            return NotFound();

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(string id, UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken = default)
        {
            if (id != updateCategoryDto.CategoryId)
                return BadRequest();

            var result =  await _categoryService.UpdateAsync(updateCategoryDto, cancellationToken);

            if (!result)
                return NotFound();

            return NoContent();

        }
    }
}
