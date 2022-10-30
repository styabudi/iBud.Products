using iBud.Products.Infrastructure.Models.Common.Mail;

namespace iBud.Products.Infrastructure.Common.Mail;
public interface IMailSender
{
    MailResult SendEmail(MailModel model);
}