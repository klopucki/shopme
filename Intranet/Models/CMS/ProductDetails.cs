using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Intranet.Models.Shop;

namespace Intranet.Models.CMS;

public class ProductDetails
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(2000)]
    [DisplayName("Product Description")]
    public string Description { get; set; }

    [MaxLength(500)]
    public string Manufacturer { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Weight { get; set; }

    // 1:1
    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    public Product Product { get; set; }
}