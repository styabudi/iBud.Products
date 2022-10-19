using System.ComponentModel.DataAnnotations;

namespace iBud.Products.Infrastructure.Models.Dtos.Input.Authentication;
public class UserRegistration
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;

}