using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class Follow
{
    public int FollowerUserId { get; set; }

    public int FollowedUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User FollowedUser { get; set; } = null!;

    public virtual User FollowerUser { get; set; } = null!;
}
