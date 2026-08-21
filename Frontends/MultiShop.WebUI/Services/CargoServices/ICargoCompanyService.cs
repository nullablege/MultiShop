using MultiShop.WebUI.Models.CargoDTOs;

namespace MultiShop.WebUI.Services.CargoServices;

public interface ICargoCompanyService
{
    Task<IReadOnlyList<ResultCargoCompanyDto>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task<UpdateCargoCompanyDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
    Task CreateAsync(
        CreateCargoCompanyDto createCargoCompanyDto,
        CancellationToken cancellationToken = default);
    Task UpdateAsync(
        UpdateCargoCompanyDto updateCargoCompanyDto,
        CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
