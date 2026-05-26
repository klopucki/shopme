using System.Text.Json;
using Client.Models;

namespace Client.Services;

public static class CartSessionExtensions
{
    private const string CartKey = "ShoppingCart";

    public static List<CartItemViewModel> GetCart(this ISession session)
    {
        var json = session.GetString(CartKey);
        return string.IsNullOrWhiteSpace(json)
            ? []
            : JsonSerializer.Deserialize<List<CartItemViewModel>>(json) ?? [];
    }

    public static void SetCart(this ISession session, List<CartItemViewModel> items)
    {
        session.SetString(CartKey, JsonSerializer.Serialize(items));
    }
}
