using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class NotificationPreference
{
    public int UserId { get; set; }

    public bool FollowNotificationsEnabled { get; set; }

    public bool ReviewLikeNotificationsEnabled { get; set; }

    public bool SubmissionNotificationsEnabled { get; set; }

    public bool FeedActivityNotificationsEnabled { get; set; }

    public virtual User User { get; set; } = null!;
}
