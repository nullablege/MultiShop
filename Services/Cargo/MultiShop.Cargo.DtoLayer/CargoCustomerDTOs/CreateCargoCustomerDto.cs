namespace MultiShop.Cargo.DtoLayer.CargoCustomerDTOs
{
    public class CreateCargoCustomerDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? UserCustomerId { get; set; }
    }
}
