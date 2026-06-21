namespace Intranet.Models;

public class DashboardViewModel
{
    public int ProductCount { get; set; }
    public int CategoryCount { get; set; }
    public int FeaturedProductCount { get; set; }
    public int OutOfStockProductCount { get; set; }
    public int LowStockProductCount { get; set; }
    public int ArticleCount { get; set; }
    public int PublishedArticleCount { get; set; }
    public int PageCount { get; set; }
    public int PublishedPageCount { get; set; }
    public int RankingCount { get; set; }
    public int PublishedRankingCount { get; set; }
    public int UserCount { get; set; }
    public int AuditLogCountToday { get; set; }
    public double AverageProductRating { get; set; }

    public List<LowStockProductItem> LowStockProducts { get; set; } = [];
    public List<RecentAuditItem> RecentAuditLogs { get; set; } = [];
    public List<RecentContentItem> RecentContent { get; set; } = [];
}

public class LowStockProductItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class RecentAuditItem
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Controller { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;
    public string? Result { get; set; }
    public DateTime Timestamp { get; set; }
    public bool HasException { get; set; }
}

public class RecentContentItem
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateTime PublishedAt { get; set; }
    public string Controller { get; set; } = string.Empty;
    public int Id { get; set; }
}
