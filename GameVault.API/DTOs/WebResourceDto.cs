namespace GameVault.API.DTOs;

public class WebResourceDto
{
    public int ResourceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
}
