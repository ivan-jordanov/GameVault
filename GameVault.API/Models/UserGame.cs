using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class UserGame
{
    public int UserId { get; set; }

    public int GameId { get; set; }

    public int StatusId { get; set; }

    public decimal PlaytimeHours { get; set; }

    public DateTime AddedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual GameStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
