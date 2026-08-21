using MultiShop.WebUI.Models.Order;

namespace MultiShop.WebUI.Services.OrderServices;

public interface IOrderAddressService
{
    Task CreateAsync(CreateOrderAddressDto createOrderAddressDto, CancellationToken cancellationToken = default);
}
