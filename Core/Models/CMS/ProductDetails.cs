using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.CMS;

public class ProductDetails : ISoftDeletable
{
    [Key]
    public int Id { get; set; }

    [DisplayName("Opis")]
    public string? Description { get; set; }

    [DisplayName("Producent")]
    public string? Manufacturer { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [DisplayName("Waga")]
    public decimal? Weight { get; set; }

    public bool IsActive { get; set; } = true;

    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
}
