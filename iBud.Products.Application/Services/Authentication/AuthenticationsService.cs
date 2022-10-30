using System.Web;
using iBud.Products.Application.Interfaces.Authentication;
using iBud.Products.Infrastructure.Common.Authentication;
using iBud.Products.Infrastructure.Common.Mail;
using iBud.Products.Infrastructure.Context;
using iBud.Products.Infrastructure.Models.Common.Mail;
using iBud.Products.Infrastructure.Models.Dtos.Input.Authentication;
using iBud.Products.Infrastructure.Models.Dtos.Output.Authentication;
using Microsoft.AspNetCore.Identity;
using static iBud.Products.Infrastructure.Common.Constants.CommonConstant;

namespace iBud.Products.Application.Services.Authentication;
public class AuthenticationsService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly AppDbContext _appDbContext;
    private readonly IMailSender _mailSender;
    public AuthenticationsService(UserManager<IdentityUser> userManager,
                                  ITokenGenerator tokenGenerator,
                                  AppDbContext appDbContext,
                                  IMailSender mailSender)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
        _appDbContext = appDbContext;
        _mailSender = mailSender;
    }

    public async Task<AuthenticationServiceResult> Register(UserRegistration model)
    {
        AuthenticationServiceResult result = new AuthenticationServiceResult();
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                var userExistByEmail = await _userManager.FindByEmailAsync(model.Email);
                var userExistByName = await _userManager.FindByNameAsync(model.Name);
                if (userExistByEmail is not null || userExistByName is not null)
                {
                    if (userExistByEmail is not null)
                        result.Errors.Add(AuthMessage.EmailExist);
                    if (userExistByName is not null)
                        result.Errors.Add(AuthMessage.UsernameExist);
                }
                else
                {
                    var user = new IdentityUser
                    {
                        UserName = model.Name,
                        Email = model.Email
                    };
                    var userIsCreated = await _userManager.CreateAsync(user, model.Password);
                    if (!userIsCreated.Succeeded)
                    {
                        foreach (var error in userIsCreated.Errors)
                        {
                            result.Errors.Add(error.Code + " - " + error.Description);
                        }
                        transaction.Rollback();
                        return result;
                    }

                    var secretCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callBackUrl = model.URLCallback.Replace("userIdValue", user.Id).Replace("codeValue", HttpUtility.UrlEncode(secretCode));
                    var emailBody = ConfirmEmailHtml(callBackUrl);
                    var sendMail = _mailSender.SendEmail(new MailModel
                    {
                        MailTo = user.Email,
                        MailSubject = AuthenticationMail.MailSubject,
                        MailBody = emailBody
                    });
                    if (!sendMail.IsSent)
                    {
                        result.Errors.AddRange(sendMail.Errors);
                        transaction.Rollback();
                        return result;
                    }

                    result.User = new AuthUserResult { Id = user.Id, Username = user.UserName, Email = user.Email };
                    transaction.Commit();
                    result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                transaction.Rollback();
            }
        }
        return result;
    }
    public async Task<AuthenticationServiceResult> Login(UserLogin model)
    {
        var result = new AuthenticationServiceResult();
        try
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist is null)
            {
                result.Errors.Add(AuthMessage.UserNotExist);
                return result;
            }

            var isCorrectUser = await _userManager.CheckPasswordAsync(userExist, model.Password);
            if (!isCorrectUser)
            {
                result.Errors.Add(AuthMessage.PasswordIncorrect);
                return result;
            }

            result.User = new AuthUserResult
            {
                Id = userExist.Id,
                Username = userExist.UserName,
                Email = userExist.Email
            };
            result.Token = _tokenGenerator.GenerateToken(userExist);
            result.IsSuccess = true;
        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
        }
        return result;
    }
    public async Task<EmailConfirmationResult> EmailConfirmation(string userId, string secretCode)
    {
        EmailConfirmationResult result = new EmailConfirmationResult();
        try
        {
            if (userId is null || secretCode is null)
            {
                result.Errors = new List<string>() { AuthMessage.InvalidParameter };
                return result;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                result.Errors = new List<string>() { AuthMessage.UserNotExist };
                return result;
            }

            var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, secretCode);
            if (!emailConfirmationResult.Succeeded)
            {
                return new EmailConfirmationResult() { Errors = emailConfirmationResult.Errors.Select(x => x.Description).ToList() };
            }
            result.IsSuccess = true;
            result.SuccessMessage = AuthMessage.UserConfirmed;
        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
        }
        return result;
    }

    protected string ConfirmEmailHtml(string urlCallBack)
    {
        var html = System.IO.File.ReadAllText(@"./Assets/Templates/Mail/EmailConfirmationTemplate.html");
        html = html.Replace("{{URLCallBack}}", urlCallBack);
        return html;
    }
}