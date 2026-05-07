namespace GameVault.API.Models;

public class Review
{
    public int ReviewId { get; set; }
    public int UserId { get; set; }
    public int GameId { get; set; }
    public int Rating { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Game Game { get; set; } = null!;
}
