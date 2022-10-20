using iBud.Products.Infrastructure.Models.Dtos.Input.Authentication;
using iBud.Products.Infrastructure.Models.Dtos.Output.Authentication;

namespace iBud.Products.Application.Interfaces.Authentication;
public interface IAuthenticationService
{
    Task<AuthenticationServiceResult> Register(UserRegistration model);
    Task<AuthenticationServiceResult> Login(UserLogin model);
}