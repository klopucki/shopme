using System.ComponentModel.DataAnnotations;

namespace Core.Models.CMS;

public class Tag
{
    [Key] public int Id { get; set; }

    [Required] [StringLength(50)] public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    //  many-to-many 
    public ICollection<ProductTag>? ProductTags { get; set; }
}
