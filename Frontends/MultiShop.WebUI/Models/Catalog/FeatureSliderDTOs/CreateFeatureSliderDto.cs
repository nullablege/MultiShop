namespace MultiShop.WebUI.Models.Catalog.FeatureSliderDTOs
{
    public class CreateFeatureSliderDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
