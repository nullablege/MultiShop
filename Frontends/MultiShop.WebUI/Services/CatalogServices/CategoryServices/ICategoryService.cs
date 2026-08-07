using MultiShop.WebUI.Models.Catalog.CategoryDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.CategoryServices
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<ResultCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UpdateCategoryDto?> GetByIdAsync(string categoryId, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(string categoryId, CancellationToken cancellationToken = default);
    }
}
