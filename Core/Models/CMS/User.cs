using System.ComponentModel.DataAnnotations;

namespace Core.Models.CMS;

public class User : ISoftDeletable
{
    [Key] public int Id { get; set; }

    [Required] [StringLength(50)] public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(255)] public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // todo
    // public ICollection<UserRole>? UserRoles { get; set; }
}
