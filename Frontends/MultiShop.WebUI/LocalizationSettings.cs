namespace MultiShop.WebUI;

public static class LocalizationSettings
{
    public const string DefaultCulture = "tr-TR";

    public static IReadOnlyList<string> SupportedCultures { get; } =
        [DefaultCulture, "en-US"];
}
