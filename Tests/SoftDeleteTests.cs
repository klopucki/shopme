using Core.Data;
using Core.Models.CMS;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class SoftDeleteTests
{
    [Fact]
    public async Task Query_filter_hides_inactive_users()
    {
        await using var context = CreateContext();
        context.User.AddRange(
            new User
            {
                Username = "active",
                Email = "active@example.com",
                PasswordHash = "hash",
                IsActive = true
            },
            new User
            {
                Username = "inactive",
                Email = "inactive@example.com",
                PasswordHash = "hash",
                IsActive = false
            });
        await context.SaveChangesAsync();

        var visibleUsers = await context.User.ToListAsync();
        var allUsers = await context.User.IgnoreQueryFilters().ToListAsync();

        var visibleUser = Assert.Single(visibleUsers);
        Assert.Equal("active", visibleUser.Username);
        Assert.Equal(2, allUsers.Count);
    }

    [Fact]
    public async Task Remove_marks_user_as_inactive_instead_of_deleting_row()
    {
        await using var context = CreateContext();
        var user = new User
        {
            Username = "admin",
            Email = "admin@example.com",
            PasswordHash = "hash",
            IsActive = true
        };
        context.User.Add(user);
        await context.SaveChangesAsync();

        context.User.Remove(user);
        await context.SaveChangesAsync();

        Assert.Empty(await context.User.ToListAsync());

        var deletedUser = await context.User
            .IgnoreQueryFilters()
            .SingleAsync();
        Assert.False(deletedUser.IsActive);
    }

    private static ShopMeDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ShopMeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ShopMeDbContext(options);
    }
}
