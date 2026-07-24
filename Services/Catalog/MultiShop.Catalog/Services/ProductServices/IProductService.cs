using MultiShop.Catalog.DTOs.ProductDTOs;

namespace MultiShop.Catalog.Services.ProductServices
{
    public interface IProductService
    {
        Task<IReadOnlyList<ResultProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GetByIdProductDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateProductDto createProductDto , CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
