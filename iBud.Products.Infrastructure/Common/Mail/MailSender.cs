using iBud.Products.Infrastructure.Common.Configuration;
using iBud.Products.Infrastructure.Models.Common.Mail;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace iBud.Products.Infrastructure.Common.Mail;
public class MailSender : IMailSender
{
    private readonly MailConfiguration _mailSender;

    public MailSender(IOptions<MailConfiguration> mailsenderOption)
    {
        _mailSender = mailsenderOption.Value;
    }

    public MailResult SendEmail(MailModel model)
    {
        MailResult result = new MailResult();
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_mailSender.DisplayName, _mailSender.From));
        email.To.Add(MailboxAddress.Parse(model.MailTo));
        email.Subject = model.MailSubject;
        email.Body = new TextPart(TextFormat.Html) { Text = model.MailBody };
        try
        {
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSender.Host, _mailSender.Port, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(_mailSender.Username, _mailSender.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
            result.IsSent = true;
        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
        }
        return result;
    }
}