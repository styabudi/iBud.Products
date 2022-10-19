namespace iBud.Products.Infrastructure.Models.Dtos.Output;
public class BaseModelResult
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}