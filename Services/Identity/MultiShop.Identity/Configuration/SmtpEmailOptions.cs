namespace MultiShop.Identity.Configuration;

public sealed class SmtpEmailOptions
{
    public const string SectionName = "Email:Smtp";

    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public bool UseStartTls { get; init; }
    public bool UseAuthentication { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string SenderName { get; init; } = "MultiShop";
    public string SenderEmail { get; init; } = string.Empty;
    public int TimeoutSeconds { get; init; } = 10;
}
