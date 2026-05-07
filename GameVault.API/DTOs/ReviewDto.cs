namespace GameVault.API.DTOs;

public class ReviewDto
{
    public int ReviewId { get; set; }
    public int GameId { get; set; }
    public string GameTitle { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
}
