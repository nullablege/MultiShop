namespace MultiShop.Cargo.DtoLayer.CargoDetailDTOs
{
    public class CreateCargoDetailDto
    {
        public string SenderCustomer { get; set; } = string.Empty;
        public string ReceiverCustomer { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public int CargoCompanyId { get; set; }
    }
}
