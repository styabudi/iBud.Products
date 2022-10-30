namespace iBud.Products.Infrastructure.Models.Dtos.Output.Authentication;

public class EmailConfirmationResult : BaseModelResult
{
    public string SuccessMessage { get; set; } = null!;
}