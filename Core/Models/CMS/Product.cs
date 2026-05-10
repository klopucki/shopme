using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.CMS;

public class Product
{
    [Key] public int Id { get; set; }

    [Required]
    [DisplayName("Nazwa produktu")]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    [DisplayName("Cena")]
    public decimal Price { get; set; }

    [DisplayName("Aktywny")] public bool IsActive { get; set; }

    [DisplayName("Data utworzenia")] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [DisplayName("Aktywny do")] public DateTime? ActiveUntil { get; set; }

    [DisplayName("Ilość sztuk")] public int Quantity { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")] public Category? Category { get; set; }

    public ProductDetails? ProductDetails { get; set; }

    // many-to-many
    public ICollection<ProductTag>? ProductTags { get; set; }

    public ICollection<ProductReview>? ProductReviews { get; set; }
}