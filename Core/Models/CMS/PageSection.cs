using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.CMS;

public class PageSection
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(60)]
    [DisplayName("Type")]
    public string Type { get; set; } = PageSectionTypes.Text;

    [StringLength(160)]
    [DisplayName("Title")]
    public string? Title { get; set; }

    [DisplayName("Content")]
    public string? Content { get; set; }

    [StringLength(500)]
    [DisplayName("Image URL")]
    public string? ImageUrl { get; set; }

    [StringLength(160)]
    [DisplayName("Button text")]
    public string? ButtonText { get; set; }

    [StringLength(500)]
    [DisplayName("Button URL")]
    public string? ButtonUrl { get; set; }

    [DisplayName("Sort order")]
    public int SortOrder { get; set; }

    [DisplayName("Active")]
    public bool IsActive { get; set; } = true;

    public int PageId { get; set; }

    [ForeignKey("PageId")]
    public Page? Page { get; set; }
}

public static class PageSectionTypes
{
    public const string Hero = "Hero";
    public const string Text = "Text";
    public const string ImageText = "ImageText";
    public const string FeaturedProducts = "FeaturedProducts";
    public const string LatestArticles = "LatestArticles";
    public const string FeaturedRankings = "FeaturedRankings";

    public static readonly IReadOnlyList<string> All =
    [
        Hero,
        Text,
        ImageText,
        FeaturedProducts,
        LatestArticles,
        FeaturedRankings
    ];
}
