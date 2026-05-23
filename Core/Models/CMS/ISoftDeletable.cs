namespace Core.Models.CMS;

public interface ISoftDeletable
{
    bool IsActive { get; set; }
}
