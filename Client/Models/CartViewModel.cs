namespace Client.Models;

public class CartViewModel
{
    public IReadOnlyList<CartItemViewModel> Items { get; init; } = [];

    public decimal Total => Items.Sum(item => item.Total);
}
