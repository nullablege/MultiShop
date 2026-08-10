using MultiShop.WebUI.Models.Catalog.BrandDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.BrandServices;

public interface IBrandService
{
    Task<IReadOnlyList<ResultBrandDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UpdateBrandDto?> GetByIdAsync(string brandId, CancellationToken cancellationToken = default);
    Task CreateAsync(CreateBrandDto createBrandDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateBrandDto updateBrandDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(string brandId, CancellationToken cancellationToken = default);
}
