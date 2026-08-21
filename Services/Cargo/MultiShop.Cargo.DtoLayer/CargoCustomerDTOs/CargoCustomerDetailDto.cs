namespace MultiShop.Cargo.DtoLayer.CargoCustomerDTOs;

public sealed class CargoCustomerDetailDto
{
    public int CargoCustomerId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string District { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string? UserCustomerId { get; init; }
}
