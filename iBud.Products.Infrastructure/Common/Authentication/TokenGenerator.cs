using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using iBud.Products.Infrastructure.Common.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace iBud.Products.Infrastructure.Common.Authentication;
public class TokenGenerator : ITokenGenerator
{
    private readonly JwtConfig _jwtConfig;
    public TokenGenerator(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public string GenerateToken(IdentityUser user)
    {
        var signinCredential = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)),
                                                      SecurityAlgorithms.HmacSha256);
        var claims = new[]{
            new Claim("Id",user.Id),
            new Claim(JwtRegisteredClaimNames.Sub,user.Email),
            new Claim(JwtRegisteredClaimNames.Name,user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString())
        };
        var securityToken = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryMinutes),
            claims: claims,
            signingCredentials: signinCredential
        );
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}