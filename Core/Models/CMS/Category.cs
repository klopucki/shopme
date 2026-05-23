using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.CMS;

public class Category
{
    [Key] public int Id { get; set; }

    [Required]
    [DisplayName("Nazwa kategorii")]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
