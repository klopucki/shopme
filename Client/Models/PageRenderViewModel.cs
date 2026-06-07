using Core.Models.CMS;
using Core.Models.Shop;

namespace Client.Models;

public class PageRenderViewModel
{
    public Page Page { get; init; } = default!;

    public IReadOnlyList<Product> FeaturedProducts { get; init; } = [];

    public IReadOnlyList<Article> LatestArticles { get; init; } = [];

    public IReadOnlyList<Ranking> FeaturedRankings { get; init; } = [];
}
