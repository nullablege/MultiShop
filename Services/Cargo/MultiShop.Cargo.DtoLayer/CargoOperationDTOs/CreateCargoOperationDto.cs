namespace MultiShop.Cargo.DtoLayer.CargoOperationDTOs
{
    public class CreateCargoOperationDto
    {
        public string Barcode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime OperationDate { get; set; }
    }
}
