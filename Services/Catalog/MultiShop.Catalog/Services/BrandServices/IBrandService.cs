using MultiShop.Catalog.DTOs.BrandDTOs;

namespace MultiShop.Catalog.Services.BrandServices;

public interface IBrandService
{
    Task<IReadOnlyList<ResultBrandDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GetByIdBrandDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task CreateAsync(CreateBrandDto createBrandDto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(UpdateBrandDto updateBrandDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
