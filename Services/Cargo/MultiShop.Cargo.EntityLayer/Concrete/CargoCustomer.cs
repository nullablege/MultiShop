namespace MultiShop.Cargo.EntityLayer.Concrete
{
    public class CargoCustomer
    {
        public int CargoCustomerId { get; set; }
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
