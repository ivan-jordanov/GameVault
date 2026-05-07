namespace GameVault.API.DTOs;

public class GameDetailDto
{
    public int GameId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverArtUrl { get; set; } = string.Empty;
    public string Developer { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public List<GameImageDto> Images { get; set; } = new List<GameImageDto>();
    public List<string> Categories { get; set; } = new List<string>();
    public List<string> Platforms { get; set; } = new List<string>();
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
}

public class GameImageDto
{
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
