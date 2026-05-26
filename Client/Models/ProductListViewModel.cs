using Core.Models.CMS;

namespace Client.Models;

public class ProductListViewModel
{
    public IReadOnlyList<Product> Products { get; init; } = [];

    public IReadOnlyList<Category> Categories { get; init; } = [];

    public IReadOnlyList<Tag> Tags { get; init; } = [];

    public int? CategoryId { get; init; }

    public int? TagId { get; init; }

    public string? Search { get; init; }
}
