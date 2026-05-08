using System.ComponentModel.DataAnnotations;

namespace Intranet.Models.CMS;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(200)]
    public string Email { get; set; }

    public ICollection<News> NewsList { get; set; }
}