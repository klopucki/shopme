using System.ComponentModel;

namespace Core.Models.CMS;

public class ArticleTagAssignment
{
    public int ArticleId { get; set; }

    public Article? Article { get; set; }

    public int ArticleTagId { get; set; }

    public ArticleTag? ArticleTag { get; set; }

    [DisplayName("Active")]
    public bool IsActive { get; set; } = true;
}
