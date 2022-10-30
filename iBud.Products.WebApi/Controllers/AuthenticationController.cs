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
        model.URLCallback = Request.Scheme + "://" + Request.Host + Url.Action("EmailConfirmation", "Authentication", new { userId = "userIdValue", secretCode = "codeValue" });
        if (ModelState.IsValid)
        {
            var userCreated = await _authService.Register(model);
            if (userCreated.IsSuccess)
                return Ok(userCreated);
            return BadRequest(userCreated);
        }
        return BadRequest();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] UserLogin model)
    {
        if (ModelState.IsValid)
        {
            var userSigned = await _authService.Login(model);
            if (userSigned.IsSuccess)
                return Ok(userSigned);
            return BadRequest(userSigned);
        }
        return BadRequest();
    }

    public async Task<IActionResult> EmailConfirmation(string userId, string secretCode)
    {
        var result = await _authService.EmailConfirmation(userId, secretCode);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return base.Content(System.IO.File.ReadAllText(@"./Assets/Templates/Mail/AccountVerified.html"), "text/html");
    }
}