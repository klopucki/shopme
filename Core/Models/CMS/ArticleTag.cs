using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.CMS;

public class ArticleTag
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [DisplayName("Name")]
    public string Name { get; set; } = string.Empty;

    [DisplayName("Active")]
    public bool IsActive { get; set; } = true;

    public ICollection<ArticleTagAssignment>? ArticleTagAssignments { get; set; }
}
