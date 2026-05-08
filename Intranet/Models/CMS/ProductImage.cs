using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Intranet.Models.Shop;

namespace Intranet.Models.CMS;

public class ProductImage
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string ImageUrl { get; set; }

    public bool IsMain { get; set; }

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    public Product Product { get; set; }
}