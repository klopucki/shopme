namespace Client.Models;

public class CartItemViewModel
{
    public int ProductId { get; init; }

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string? ImageUrl { get; init; }

    public int Quantity { get; set; }

    public decimal Total => Price * Quantity;
}
