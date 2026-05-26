using Core.Data;
using Core.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers;

public class ArticleController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var articles = await context.Article
            .Include(item => item.ArticleCategory)
            .OrderByDescending(item => item.PublishedAt)
            .ToListAsync();

        return View(articles);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await context.Article
            .Include(item => item.ArticleCategory)
            .Include(item => item.ArticleTagAssignments!)
                .ThenInclude(item => item.ArticleTag)
            .FirstOrDefaultAsync(item => item.Id == id);

        return article == null ? NotFound() : View(article);
    }

    public async Task<IActionResult> Create()
    {
        await LoadArticleLookups();
        return View(new Article { IsActive = true, IsPublished = true, PublishedAt = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Slug,Summary,Content,ImageUrl,IsFeatured,IsPublished,PublishedAt,IsActive,CreatedAt,ArticleCategoryId")] Article article, List<int>? selectedTagIds)
    {
        article.Slug = BuildSlug(article.Slug, article.Title);
        article.CreatedAt = article.CreatedAt == default ? DateTime.Now : article.CreatedAt;

        if (ModelState.IsValid)
        {
            context.Add(article);
            await context.SaveChangesAsync();
            await UpdateTags(article.Id, selectedTagIds);
            return RedirectToAction(nameof(Index));
        }

        await LoadArticleLookups(selectedTagIds);
        return View(article);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await context.Article
            .Include(item => item.ArticleTagAssignments)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (article == null)
        {
            return NotFound();
        }

        await LoadArticleLookups(article.ArticleTagAssignments?.Select(item => item.ArticleTagId).ToList());
        return View(article);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Slug,Summary,Content,ImageUrl,IsFeatured,IsPublished,PublishedAt,IsActive,CreatedAt,ArticleCategoryId")] Article article, List<int>? selectedTagIds)
    {
        if (id != article.Id)
        {
            return NotFound();
        }

        article.Slug = BuildSlug(article.Slug, article.Title);

        if (ModelState.IsValid)
        {
            var articleToUpdate = await context.Article.FindAsync(id);
            if (articleToUpdate == null)
            {
                return NotFound();
            }

            context.Entry(articleToUpdate).CurrentValues.SetValues(article);
            await context.SaveChangesAsync();
            await UpdateTags(article.Id, selectedTagIds);
            return RedirectToAction(nameof(Index));
        }

        await LoadArticleLookups(selectedTagIds);
        return View(article);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var article = await context.Article
            .Include(item => item.ArticleCategory)
            .FirstOrDefaultAsync(item => item.Id == id);

        return article == null ? NotFound() : View(article);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var article = await context.Article
            .Include(item => item.ArticleTagAssignments)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (article != null)
        {
            context.Article.Remove(article);

            if (article.ArticleTagAssignments != null)
            {
                context.ArticleTagAssignment.RemoveRange(article.ArticleTagAssignments);
            }

            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadArticleLookups(List<int>? selectedTagIds = null)
    {
        ViewData["ArticleCategoryId"] = new SelectList(await context.ArticleCategory.OrderBy(item => item.Name).ToListAsync(), "Id", "Name");
        ViewBag.ArticleTags = await context.ArticleTag.OrderBy(item => item.Name).ToListAsync();
        ViewBag.SelectedTagIds = selectedTagIds ?? [];
    }

    private async Task UpdateTags(int articleId, List<int>? selectedTagIds)
    {
        selectedTagIds ??= [];

        var assignments = await context.ArticleTagAssignment
            .IgnoreQueryFilters()
            .Where(item => item.ArticleId == articleId)
            .ToListAsync();

        var existingTagIds = assignments.Select(item => item.ArticleTagId).ToList();

        foreach (var assignment in assignments)
        {
            assignment.IsActive = selectedTagIds.Contains(assignment.ArticleTagId);
        }

        foreach (var tagId in selectedTagIds.Where(tagId => !existingTagIds.Contains(tagId)))
        {
            context.ArticleTagAssignment.Add(new ArticleTagAssignment { ArticleId = articleId, ArticleTagId = tagId });
        }

        await context.SaveChangesAsync();
    }

    private static string BuildSlug(string? slug, string fallback)
    {
        var source = string.IsNullOrWhiteSpace(slug) ? fallback : slug;
        return string.Join("-", source.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}
