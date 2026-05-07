using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class ActivityFeed
{
    public int ActivityId { get; set; }

    public int UserId { get; set; }

    public string ActivityType { get; set; } = null!;

    public string ReferenceType { get; set; } = null!;

    public int ReferenceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
