namespace MultiShop.Cargo.EntityLayer.Concrete
{
    public class CargoDetail
    {
        public int CargoDetailId { get; set; }
        public string SenderCustomer { get; set; } = string.Empty;
        public string ReceiverCustomer { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public int CargoCompanyId { get; set; }
        public CargoCompany? CargoCompany { get; set; }
    }
}
