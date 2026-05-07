namespace GameVault.API.Models;

public class News
{
    public int NewsId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public bool IsPublished { get; set; }
    public int? LastModifiedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User? LastModifiedByUser { get; set; }
}
