using Client.Models;
using Client.Services;

namespace Tests;

public class CartSessionTests
{
    [Fact]
    public void GetCart_returns_empty_list_when_session_has_no_cart()
    {
        var session = new FakeSession();

        var cart = session.GetCart();

        Assert.Empty(cart);
    }

    [Fact]
    public void SetCart_saves_cart_items_as_session_data()
    {
        var session = new FakeSession();
        var items = new List<CartItemViewModel>
        {
            new()
            {
                ProductId = 10,
                Name = "Keyboard",
                Price = 199.99m,
                ImageUrl = "/images/keyboard.png",
                Quantity = 2
            }
        };

        session.SetCart(items);
        var cart = session.GetCart();

        var item = Assert.Single(cart);
        Assert.Equal(10, item.ProductId);
        Assert.Equal("Keyboard", item.Name);
        Assert.Equal(199.99m, item.Price);
        Assert.Equal("/images/keyboard.png", item.ImageUrl);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(399.98m, item.Total);
    }
}
