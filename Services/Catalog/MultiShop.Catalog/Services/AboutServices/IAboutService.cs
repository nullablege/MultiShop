using MultiShop.Catalog.DTOs.AboutDTOs;

namespace MultiShop.Catalog.Services.AboutServices;

public interface IAboutService
{
    Task<IReadOnlyList<ResultAboutDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GetByIdAboutDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task CreateAsync(CreateAboutDto createAboutDto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(UpdateAboutDto updateAboutDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
