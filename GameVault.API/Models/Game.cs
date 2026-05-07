namespace GameVault.API.Models;

public class Game
{
    public int GameId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverArtUrl { get; set; } = string.Empty;
    public string Developer { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<GameImage> GameImages { get; set; } = new List<GameImage>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
