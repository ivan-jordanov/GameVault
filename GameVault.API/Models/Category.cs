namespace GameVault.API.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Game> Games { get; set; } = new List<Game>();
}
