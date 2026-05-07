namespace GameVault.API.Models;

public class WebResource
{
    public int ResourceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public int? LastModifiedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User? LastModifiedByUser { get; set; }
}
