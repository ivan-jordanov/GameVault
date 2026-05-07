using System;
using System.Collections.Generic;

namespace GameVault.API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? ProfileImageUrl { get; set; }

    public string? Bio { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<ActivityFeed> ActivityFeeds { get; set; } = new List<ActivityFeed>();

    public virtual ICollection<FavoriteGame> FavoriteGames { get; set; } = new List<FavoriteGame>();

    public virtual ICollection<Follow> FollowFollowedUsers { get; set; } = new List<Follow>();

    public virtual ICollection<Follow> FollowFollowerUsers { get; set; } = new List<Follow>();

    public virtual ICollection<GameSubmission> GameSubmissionReviewedByUsers { get; set; } = new List<GameSubmission>();

    public virtual ICollection<GameSubmission> GameSubmissionSubmittedByUsers { get; set; } = new List<GameSubmission>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual NotificationPreference? NotificationPreference { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

    public virtual ICollection<ReviewLike> ReviewLikes { get; set; } = new List<ReviewLike>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();

    public virtual ICollection<WebResource> WebResources { get; set; } = new List<WebResource>();

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
