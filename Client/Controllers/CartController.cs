using Client.Models;
using Client.Services;
using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.Controllers;

public class CartController(ShopMeDbContext context) : Controller
{
    public IActionResult Index()
    {
        return View(new CartViewModel { Items = HttpContext.Session.GetCart() });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId)
    {
        var product = await context.Product.FirstOrDefaultAsync(item => item.Id == productId);
        if (product == null)
        {
            return NotFound();
        }

        var cart = HttpContext.Session.GetCart();
        var item = cart.FirstOrDefault(cartItem => cartItem.ProductId == product.Id);

        if (item == null)
        {
            cart.Add(new CartItemViewModel
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = 1
            });
        }
        else
        {
            item.Quantity++;
        }

        HttpContext.Session.SetCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        var cart = HttpContext.Session.GetCart();
        cart.RemoveAll(item => item.ProductId == productId);
        HttpContext.Session.SetCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Clear()
    {
        HttpContext.Session.SetCart([]);
        return RedirectToAction(nameof(Index));
    }
}
