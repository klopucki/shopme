using Core.Models.CMS;

namespace Client.Models;

public class HomePageViewModel
{
    public IReadOnlyList<Category> Categories { get; init; } = [];

    public IReadOnlyList<Product> FeaturedProducts { get; init; } = [];

    public IReadOnlyList<Article> LatestArticles { get; init; } = [];

    public IReadOnlyList<Ranking> FeaturedRankings { get; init; } = [];
}
