using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.CMS;

public class Ranking
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(140)]
    [DisplayName("Title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(180)]
    [DisplayName("Slug")]
    public string Slug { get; set; } = string.Empty;

    [StringLength(360)]
    [DisplayName("Summary")]
    public string? Summary { get; set; }

    [DisplayName("Featured")]
    public bool IsFeatured { get; set; }

    [DisplayName("Published")]
    public bool IsPublished { get; set; } = true;

    [DisplayName("Published at")]
    public DateTime PublishedAt { get; set; } = DateTime.Now;

    [DisplayName("Active")]
    public bool IsActive { get; set; } = true;

    public ICollection<RankingItem>? RankingItems { get; set; }
}
