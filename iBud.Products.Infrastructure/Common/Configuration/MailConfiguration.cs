namespace iBud.Products.Infrastructure.Common.Configuration;

public class MailConfiguration
{
    public const string SectionName = "SmtpSettings";
    public string Host { get; init; } = null!;
    public string From { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public int Port { get; set; }
    public string DisplayName { get; init; } = null!;
}