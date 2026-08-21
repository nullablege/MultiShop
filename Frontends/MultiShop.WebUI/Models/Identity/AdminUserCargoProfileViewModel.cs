using MultiShop.WebUI.Models.CargoDTOs;

namespace MultiShop.WebUI.Models.Identity;

public sealed class AdminUserCargoProfileViewModel
{
    public AdminUserListItemDto User { get; init; } = new();
    public CargoCustomerDetailDto? CargoCustomer { get; init; }
}
