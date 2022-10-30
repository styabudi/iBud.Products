namespace iBud.Products.Infrastructure.Models.Common.Mail;
public class MailModel
{
    public string MailTo { get; set; } = null!;
    public string MailFrom { get; set; } = null!;
    public string MailCC { get; set; } = null!;
    public string MailSubject { get; set; } = null!;
    public string MailBody { get; set; } = null!;
}