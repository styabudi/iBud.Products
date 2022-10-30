namespace iBud.Products.Infrastructure.Models.Common.Mail;
public class MailResult
{
    public bool IsSent { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}