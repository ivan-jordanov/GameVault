namespace GameVault.API.DTOs;

public class NewsSummaryDto
{
    public int NewsId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
}
