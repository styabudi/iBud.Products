using iBud.Products.Application.Interfaces.Authentication;
using iBud.Products.Infrastructure.Models.Dtos.Input.Authentication;
using iBud.Products.Infrastructure.Models.Dtos.Output.Authentication;
using Microsoft.AspNetCore.Identity;

namespace iBud.Products.Application.Services.Authentication;
public class AuthenticationsService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    public AuthenticationsService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AuthenticationServiceResult> Register(UserRegistration model)
    {
        AuthenticationServiceResult result = new AuthenticationServiceResult();
        try
        {
            var userExistByEmail = await _userManager.FindByEmailAsync(model.Email);
            var userExistByName = await _userManager.FindByNameAsync(model.Name);
            if (userExistByEmail is not null || userExistByName is not null)
            {
                if (userExistByEmail is not null)
                    result.Errors.Add("Email already exist");

                if (userExistByName is not null)
                    result.Errors.Add("Name already exist");
            }
            else
            {
                var user = new IdentityUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                var userIsCreated = await _userManager.CreateAsync(user, model.Password);
                if (userIsCreated.Succeeded)
                {
                    result.User = user;
                    result.IsSuccess = true;
                }
                else
                {
                    foreach (var err in userIsCreated.Errors)
                    {
                        result.Errors.Add("Error Code : " + err.Code + " - " + err.Description);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
        }
        return result;
    }
}