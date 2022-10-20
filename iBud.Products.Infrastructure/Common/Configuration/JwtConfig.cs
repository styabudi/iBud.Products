namespace iBud.Products.Infrastructure.Common.Configuration;
public class JwtConfig
{
    public const string SectionName = "JwtConfig";
    public string Secret { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpiryMinutes { get; set; }

}