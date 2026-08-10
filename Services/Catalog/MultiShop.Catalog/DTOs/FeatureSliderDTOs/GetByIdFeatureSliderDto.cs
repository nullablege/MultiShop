namespace MultiShop.Catalog.DTOs.FeatureSliderDTOs
{
    public class GetByIdFeatureSliderDto
    {
        public string FeatureSliderId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
