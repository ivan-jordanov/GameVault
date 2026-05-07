namespace GameVault.API.DTOs;

public class GameSummaryDto
{
    public int GameId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CoverArtUrl { get; set; } = string.Empty;
    public string Developer { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public List<string> Categories { get; set; } = new List<string>();
    public List<string> Platforms { get; set; } = new List<string>();
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
}
