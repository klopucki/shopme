using Client.Models;
using Client.Services;
using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.Controllers;

public class ProductController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index(int? categoryId, int? tagId, string? search)
    {
        var productsQuery = context.Product
            .Include(product => product.Category)
            .Include(product => product.ProductTags!)
                .ThenInclude(productTag => productTag.Tag)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            productsQuery = productsQuery.Where(product => product.CategoryId == categoryId.Value);
        }

        if (tagId.HasValue)
        {
            productsQuery = productsQuery.Where(product => product.ProductTags!.Any(productTag => productTag.TagId == tagId.Value));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            productsQuery = productsQuery.Where(product => product.Name.Contains(search));
        }

        var model = new ProductListViewModel
        {
            Products = await productsQuery
                .OrderByDescending(product => product.IsFeatured)
                .ThenByDescending(product => product.CreatedAt)
                .ToListAsync(),
            Categories = await context.Category.OrderBy(category => category.Name).ToListAsync(),
            Tags = await context.Tag.OrderBy(tag => tag.Name).ToListAsync(),
            CategoryId = categoryId,
            TagId = tagId,
            Search = search
        };

        return View(model);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Product
            .Include(item => item.Category)
            .Include(item => item.ProductDetails)
            .Include(item => item.ProductReviews)
            .Include(item => item.ProductTags!)
                .ThenInclude(item => item.Tag)
            .FirstOrDefaultAsync(item => item.Id == id);

        return product == null ? NotFound() : View(product);
    }
}
