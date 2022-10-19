using Microsoft.AspNetCore.Identity;

namespace iBud.Products.Infrastructure.Models.Dtos.Output.Authentication;
public class AuthenticationServiceResult : BaseModelResult
{
    public IdentityUser User { get; set; } = null!;
    public string Token { get; set; } = null!;
}