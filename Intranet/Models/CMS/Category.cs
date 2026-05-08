using System.ComponentModel.DataAnnotations;
using Intranet.Models.Shop;

namespace Intranet.Models.CMS;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Pole Nazwa jest wymagane")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Product>? Products { get; set; }
}