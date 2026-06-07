using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Shop;

public class RankingItem
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(140)]
    [DisplayName("Title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(360)]
    [DisplayName("Summary")]
    public string? Summary { get; set; }

    [StringLength(500)]
    [DisplayName("URL")]
    public string? Url { get; set; }

    [DisplayName("Score")]
    public decimal Score { get; set; }

    [DisplayName("Position")]
    public int Position { get; set; }

    [DisplayName("Active")]
    public bool IsActive { get; set; } = true;

    public int RankingId { get; set; }

    [ForeignKey("RankingId")]
    public Ranking? Ranking { get; set; }
}

