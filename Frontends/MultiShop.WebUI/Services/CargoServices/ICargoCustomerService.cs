using MultiShop.WebUI.Models.CargoDTOs;

namespace MultiShop.WebUI.Services.CargoServices;

public interface ICargoCustomerService
{
    Task<CargoCustomerDetailDto?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);
}
