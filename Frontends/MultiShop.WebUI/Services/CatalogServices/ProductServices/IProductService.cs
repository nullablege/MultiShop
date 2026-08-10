using MultiShop.WebUI.Models.Catalog.ProductDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.ProductServices
{
    public interface IProductService
    {
        Task<IReadOnlyList<ResultProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ResultProductWithCategoryDto>> GetWithCategoryAsync(CancellationToken cancellationToken = default);
        Task<UpdateProductDto?> GetByIdAsync(string productId, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateProductDto createProductDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(string productId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ResultProductDto>> GetByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default);
    }
}
