using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Core.Data; // Added for ShopMeDbContext
using Core.Models.CMS; // Added for AuditLog
using Microsoft.AspNetCore.Http; // Added for HttpContext
using System.Text.Json; // Added for JsonSerializer

namespace Intranet.Filters
{
    public class LogActionFilter(ILogger<LogActionFilter> logger, ShopMeDbContext shopMeDbContext) : IActionFilter
    {
        private Stopwatch? _stopwatch;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();

            // Only log non-GET requests to the database
            if (context.HttpContext.Request.Method != HttpMethods.Get)
            {
                var auditLog = new AuditLog
                {
                    Username = context.HttpContext.User.Identity?.Name ??
                               "Anonymous", // TODO: Get authenticated user or "Anonymous"
                    Action = context.ActionDescriptor.DisplayName ?? "Unknown Action",
                    Controller = context.Controller?.GetType().Name ?? "Unknown Controller",
                    Timestamp = DateTime.UtcNow,
                    HttpMethod = context.HttpContext.Request.Method,
                    Parameters = JsonSerializer.Serialize(context.ActionArguments) // Serialize action arguments
                };
                context.HttpContext.Items["AuditLogEntry"] = auditLog;
            }

            logger.LogInformation(
                "Executing action {ActionName} in controller {ControllerName} with arguments: {Arguments}",
                context.ActionDescriptor.DisplayName,
                context.Controller?.GetType().Name,
                context.ActionArguments);
        }

        public void OnActionExecuted(ActionExecutedContext context1)
        {
            if (_stopwatch != null)
            {
                _stopwatch.Stop();
                logger.LogInformation(
                    "Executed action {ActionName} in controller {ControllerName} in {ElapsedMilliseconds}ms. Result: {ResultType}",
                    context1.ActionDescriptor.DisplayName,
                    context1.Controller?.GetType().Name,
                    _stopwatch.ElapsedMilliseconds,
                    context1.Result?.GetType().Name);

                // Only log non-GET requests to the database
                if (context1.HttpContext.Request.Method != HttpMethods.Get)
                {
                    if (context1.HttpContext.Items.TryGetValue("AuditLogEntry", out var value) &&
                        value is AuditLog auditLog)
                    {
                        auditLog.Result = context1.Result?.GetType().Name;

                        if (context1.Exception != null)
                        {
                            auditLog.ExceptionDetails = context1.Exception.ToString();
                            logger.LogError(context1.Exception,
                                "Action {ActionName} in controller {ControllerName} threw an exception.",
                                context1.ActionDescriptor.DisplayName,
                                context1.Controller?.GetType().Name);
                        }

                        try
                        {
                            shopMeDbContext.AuditLog.Add(auditLog);
                            shopMeDbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Failed to save audit log for action {ActionName}.", auditLog.Action);
                        }
                    }
                }
            }
        }
    }
}