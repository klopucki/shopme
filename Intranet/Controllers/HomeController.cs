using System.Diagnostics;
using Core.Data;
using Core.Models;
using Intranet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Intranet.Controllers;

public class HomeController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.Date;

        var recentArticles = await context.Article
            .OrderByDescending(article => article.PublishedAt)
            .Take(4)
            .Select(article => new RecentContentItem
            {
                Type = "Article",
                Title = article.Title,
                IsPublished = article.IsPublished,
                PublishedAt = article.PublishedAt,
                Controller = "Article",
                Id = article.Id
            })
            .ToListAsync();

        var recentPages = await context.Page
            .OrderByDescending(page => page.PublishedAt)
            .Take(4)
            .Select(page => new RecentContentItem
            {
                Type = "Page",
                Title = page.Title,
                IsPublished = page.IsPublished,
                PublishedAt = page.PublishedAt,
                Controller = "Page",
                Id = page.Id
            })
            .ToListAsync();

        var recentRankings = await context.Ranking
            .OrderByDescending(ranking => ranking.PublishedAt)
            .Take(4)
            .Select(ranking => new RecentContentItem
            {
                Type = "Ranking",
                Title = ranking.Title,
                IsPublished = ranking.IsPublished,
                PublishedAt = ranking.PublishedAt,
                Controller = "Ranking",
                Id = ranking.Id
            })
            .ToListAsync();

        var model = new DashboardViewModel
        {
            ProductCount = await context.Product.CountAsync(),
            CategoryCount = await context.Category.CountAsync(),
            FeaturedProductCount = await context.Product.CountAsync(product => product.IsFeatured),
            OutOfStockProductCount = await context.Product.CountAsync(product => product.Quantity <= 0),
            LowStockProductCount = await context.Product.CountAsync(product => product.Quantity > 0 && product.Quantity <= 5),
            ArticleCount = await context.Article.CountAsync(),
            PublishedArticleCount = await context.Article.CountAsync(article => article.IsPublished),
            PageCount = await context.Page.CountAsync(),
            PublishedPageCount = await context.Page.CountAsync(page => page.IsPublished),
            RankingCount = await context.Ranking.CountAsync(),
            PublishedRankingCount = await context.Ranking.CountAsync(ranking => ranking.IsPublished),
            UserCount = await context.User.CountAsync(),
            AuditLogCountToday = await context.AuditLog.CountAsync(log => log.Timestamp >= today),
            AverageProductRating = await context.ProductReview.AnyAsync()
                ? await context.ProductReview.AverageAsync(review => review.Rating)
                : 0,
            LowStockProducts = await context.Product
                .Include(product => product.Category)
                .Where(product => product.Quantity <= 5)
                .OrderBy(product => product.Quantity)
                .ThenBy(product => product.Name)
                .Take(6)
                .Select(product => new LowStockProductItem
                {
                    Id = product.Id,
                    Name = product.Name,
                    CategoryName = product.Category != null ? product.Category.Name : null,
                    Quantity = product.Quantity,
                    Price = product.Price
                })
                .ToListAsync(),
            RecentAuditLogs = await context.AuditLog
                .OrderByDescending(log => log.Timestamp)
                .Take(6)
                .Select(log => new RecentAuditItem
                {
                    Id = log.Id,
                    Username = log.Username,
                    Controller = log.Controller,
                    HttpMethod = log.HttpMethod,
                    Result = log.Result,
                    Timestamp = log.Timestamp,
                    HasException = log.ExceptionDetails != null
                })
                .ToListAsync(),
            RecentContent = recentArticles
                .Concat(recentPages)
                .Concat(recentRankings)
                .OrderByDescending(item => item.PublishedAt)
                .Take(8)
                .ToList()
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
