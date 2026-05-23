using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.CMS;

public class ProductReview : ISoftDeletable
{
    [Key] public int Id { get; set; }

    [Required] public int ProductId { get; set; }

    [ForeignKey("ProductId")] public Product? Product { get; set; }

    [Required] [StringLength(100)] public string ReviewerName { get; set; } = string.Empty;

    [Range(1, 5)] public int Rating { get; set; }

    [StringLength(1000)] public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
