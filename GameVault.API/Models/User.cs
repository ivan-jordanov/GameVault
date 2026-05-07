namespace GameVault.API.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<News> ModifiedNews { get; set; } = new List<News>();
    public ICollection<WebResource> ModifiedWebResources { get; set; } = new List<WebResource>();
}
