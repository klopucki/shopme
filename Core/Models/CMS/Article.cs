using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.CMS;

public class Article
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(160)]
    [DisplayName("Title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(180)]
    [DisplayName("Slug")]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [StringLength(360)]
    [DisplayName("Summary")]
    public string Summary { get; set; } = string.Empty;

    [Required]
    [DisplayName("Content")]
    public string Content { get; set; } = string.Empty;

    [StringLength(500)]
    [DisplayName("Image URL")]
    public string? ImageUrl { get; set; }

    [DisplayName("Featured")]
    public bool IsFeatured { get; set; }

    [DisplayName("Published")]
    public bool IsPublished { get; set; } = true;

    [DisplayName("Published at")]
    public DateTime PublishedAt { get; set; } = DateTime.Now;

    [DisplayName("Active")]
    public bool IsActive { get; set; } = true;

    [DisplayName("Created at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int ArticleCategoryId { get; set; }

    [ForeignKey("ArticleCategoryId")]
    public ArticleCategory? ArticleCategory { get; set; }

    public ICollection<ArticleTagAssignment>? ArticleTagAssignments { get; set; }
}
