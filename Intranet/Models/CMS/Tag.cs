using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Intranet.Models.CMS;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [DisplayName("Tag Name")]
    public string Name { get; set; }

    public ICollection<ProductTag> ProductTags { get; set; }
}