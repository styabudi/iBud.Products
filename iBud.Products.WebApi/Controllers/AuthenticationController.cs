using iBud.Products.Application.Interfaces.Authentication;
using iBud.Products.Infrastructure.Models.Dtos.Input.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iBud.Products.WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAuthenticationService _authService;
    public AuthenticationController(UserManager<IdentityUser> userManager, IAuthenticationService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistration model)
    {
        if (ModelState.IsValid)
        {
            var userCreated = await _authService.Register(model);
            if (userCreated.IsSuccess)
                return Ok(userCreated);
            return BadRequest(userCreated);
        }
        return BadRequest();
    }
}