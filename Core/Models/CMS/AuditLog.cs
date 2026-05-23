using System.ComponentModel.DataAnnotations;

namespace Core.Models.CMS;

public class AuditLog
{
    [Key] public int Id { get; set; }

    [Required] public string Username { get; set; } = "Anonymous";

    [Required] public string Action { get; set; } = string.Empty;

    [Required] public string Controller { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string? Parameters { get; set; } // JSON 

    public string? Result { get; set; } // JSON 

    public string? ExceptionDetails { get; set; }

    [Required] public string HttpMethod { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true; // not necessery for audit but leave it to be consistent with other models
}
