using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Models.CMS;

public class News
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    //  *:1
    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }

    public User Author { get; set; }
}