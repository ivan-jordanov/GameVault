using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class Game
{
    public int GameId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? CoverArtUrl { get; set; }

    public string? Developer { get; set; }

    public string? Publisher { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<FavoriteGame> FavoriteGames { get; set; } = new List<FavoriteGame>();

    public virtual ICollection<GameImage> GameImages { get; set; } = new List<GameImage>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
