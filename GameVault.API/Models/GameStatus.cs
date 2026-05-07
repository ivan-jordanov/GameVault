using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class GameStatus
{
    public int StatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
}
