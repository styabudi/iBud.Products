using System.ComponentModel.DataAnnotations;

namespace iBud.Products.Infrastructure.Models.Common;

public class BaseModel
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    [Required]
    public string CreatedBy { get; set; } = null!;
    public string ModifiedBy { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}