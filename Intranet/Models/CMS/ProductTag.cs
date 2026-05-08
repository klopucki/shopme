using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Intranet.Models.Shop;

namespace Intranet.Models.CMS;

public class ProductTag
{
    [Key] 
    public int Id { get; set; }
    
    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    public Product Product { get; set; }

    [ForeignKey(nameof(Tag))]
    public int TagId { get; set; }

    public Tag Tag { get; set; }
}