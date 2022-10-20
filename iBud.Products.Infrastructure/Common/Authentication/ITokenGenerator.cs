using Microsoft.AspNetCore.Identity;

namespace iBud.Products.Infrastructure.Common.Authentication;
public interface ITokenGenerator
{
    string GenerateToken(IdentityUser user);
}