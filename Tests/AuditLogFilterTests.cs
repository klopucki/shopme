using System.Reflection;
using Intranet.Filters;
using Intranet.Models;

namespace Tests;

public class AuditLogFilterTests
{
    [Fact]
    public void Sanitizer_redacts_top_level_password_argument()
    {
        var actionArguments = new Dictionary<string, object?>
        {
            ["password"] = "secret",
            ["username"] = "admin"
        };

        var sanitized = Sanitize(actionArguments);

        Assert.Equal("[REDACTED]", sanitized["password"]);
        Assert.Equal("admin", sanitized["username"]);
    }

    [Fact]
    public void Sanitizer_redacts_password_property_inside_view_model()
    {
        var actionArguments = new Dictionary<string, object?>
        {
            ["model"] = new LoginViewModel
            {
                UsernameOrEmail = "admin@example.com",
                Password = "secret",
                RememberMe = true,
                ReturnUrl = "/Product"
            }
        };

        var sanitized = Sanitize(actionArguments);
        var model = Assert.IsType<Dictionary<string, object?>>(sanitized["model"]);

        Assert.Equal("admin@example.com", model["UsernameOrEmail"]);
        Assert.Equal("[REDACTED]", model["Password"]);
        Assert.Equal(true, model["RememberMe"]);
        Assert.Equal("/Product", model["ReturnUrl"]);
    }

    private static Dictionary<string, object?> Sanitize(IDictionary<string, object?> actionArguments)
    {
        var method = typeof(LogActionFilter).GetMethod(
            "SanitizeActionArguments",
            BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);

        return Assert.IsType<Dictionary<string, object?>>(
            method.Invoke(null, [actionArguments]));
    }
}
