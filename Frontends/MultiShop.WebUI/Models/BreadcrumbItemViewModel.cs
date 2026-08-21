namespace MultiShop.WebUI.Models;

public sealed record BreadcrumbItemViewModel(
    string Text,
    string? Controller = null,
    string? Action = null);
