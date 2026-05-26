using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.CMS;

public class ArticleCategory
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [DisplayName("Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    [DisplayName("Slug")]
    public string Slug { get; set; } = string.Empty;

    [DisplayName("Active")]
    public bool IsActive { get; set; } = true;

    public ICollection<Article>? Articles { get; set; }
}
