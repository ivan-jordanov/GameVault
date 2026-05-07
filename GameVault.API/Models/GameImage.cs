namespace GameVault.API.Models;

public class GameImage
{
    public int ImageId { get; set; }
    public int GameId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    // Navigation properties
    public Game Game { get; set; } = null!;
}
