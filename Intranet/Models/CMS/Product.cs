using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Intranet.Models.CMS;

namespace Intranet.Models.Shop;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DisplayName("Nazwa produktu")]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    [DisplayName("Cena")]
    public decimal Price { get; set; }

    [DisplayName("Aktywny")]
    public bool IsActive { get; set; }

    [DisplayName("Data utworzenia")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [DisplayName("Aktywny do")]
    public DateTime? ActiveUntil { get; set; }

    [DisplayName("Ilość sztuk")]
    public int Quantity { get; set; }

    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ProductDetails? Details { get; set; }
}