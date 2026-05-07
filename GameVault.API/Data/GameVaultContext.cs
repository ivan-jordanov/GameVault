using System;
using System.Collections.Generic;
using GameVault.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Data;

public partial class GameVaultContext : DbContext
{
    public GameVaultContext()
    {
    }

    public GameVaultContext(DbContextOptions<GameVaultContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityFeed> ActivityFeeds { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<FavoriteGame> FavoriteGames { get; set; }

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameImage> GameImages { get; set; }

    public virtual DbSet<GameStatus> GameStatuses { get; set; }

    public virtual DbSet<GameSubmission> GameSubmissions { get; set; }

    public virtual DbSet<GameSubmissionImage> GameSubmissionImages { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationPreference> NotificationPreferences { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ReviewLike> ReviewLikes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGame> UserGames { get; set; }

    public virtual DbSet<WebResource> WebResources { get; set; }

 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityFeed>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Activity__45F4A7910319ACFA");

            entity.ToTable("ActivityFeed");

            entity.Property(e => e.ActivityType).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReferenceType).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.ActivityFeeds)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ActivityF__UserI__160F4887");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B679E4552");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "UQ__Category__737584F6BEFB0B8A").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<FavoriteGame>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.GameId }).HasName("PK__Favorite__D52345337C917A4B");

            entity.ToTable("FavoriteGame");

            entity.HasIndex(e => new { e.UserId, e.Position }, "UQ__Favorite__922079C6B5DE6587").IsUnique();

            entity.HasOne(d => d.Game).WithMany(p => p.FavoriteGames)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FavoriteG__GameI__71D1E811");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteGames)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FavoriteG__UserI__70DDC3D8");
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => new { e.FollowerUserId, e.FollowedUserId }).HasName("PK__Follow__FA21448E89E314A9");

            entity.ToTable("Follow");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.FollowedUser).WithMany(p => p.FollowFollowedUsers)
                .HasForeignKey(d => d.FollowedUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Follow__Followed__114A936A");

            entity.HasOne(d => d.FollowerUser).WithMany(p => p.FollowFollowerUsers)
                .HasForeignKey(d => d.FollowerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Follow__Follower__10566F31");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Game__2AB897FDC20575EC");

            entity.ToTable("Game");

            entity.Property(e => e.CoverArtUrl).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Developer).HasMaxLength(150);
            entity.Property(e => e.Publisher).HasMaxLength(150);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasMany(d => d.Categories).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GameCateg__Categ__5EBF139D"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .HasConstraintName("FK__GameCateg__GameI__5DCAEF64"),
                    j =>
                    {
                        j.HasKey("GameId", "CategoryId").HasName("PK__GameCate__8B28045D582E03C6");
                        j.ToTable("GameCategory");
                    });

            entity.HasMany(d => d.Platforms).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GamePlatform",
                    r => r.HasOne<Platform>().WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GamePlatf__Platf__628FA481"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .HasConstraintName("FK__GamePlatf__GameI__619B8048"),
                    j =>
                    {
                        j.HasKey("GameId", "PlatformId").HasName("PK__GamePlat__95ED089240102CA9");
                        j.ToTable("GamePlatform");
                    });
        });

        modelBuilder.Entity<GameImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__GameImag__7516F70C4782F602");

            entity.ToTable("GameImage");

            entity.Property(e => e.ImageUrl).HasMaxLength(255);

            entity.HasOne(d => d.Game).WithMany(p => p.GameImages)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__GameImage__GameI__59FA5E80");
        });

        modelBuilder.Entity<GameStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__GameStat__C8EE20634D06CAD6");

            entity.ToTable("GameStatus");

            entity.HasIndex(e => e.Name, "UQ__GameStat__737584F6E0C5E3A6").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<GameSubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("PK__GameSubm__449EE125D05D44BC");

            entity.ToTable("GameSubmission");

            entity.Property(e => e.CoverArtUrl).HasMaxLength(255);
            entity.Property(e => e.Developer).HasMaxLength(150);
            entity.Property(e => e.Publisher).HasMaxLength(150);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.ReviewedAt).HasColumnType("datetime");
            entity.Property(e => e.SubmissionStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.ReviewedByUser).WithMany(p => p.GameSubmissionReviewedByUsers)
                .HasForeignKey(d => d.ReviewedByUserId)
                .HasConstraintName("FK__GameSubmi__Revie__693CA210");

            entity.HasOne(d => d.SubmittedByUser).WithMany(p => p.GameSubmissionSubmittedByUsers)
                .HasForeignKey(d => d.SubmittedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GameSubmi__Submi__656C112C");
        });

        modelBuilder.Entity<GameSubmissionImage>(entity =>
        {
            entity.HasKey(e => e.SubmissionImageId).HasName("PK__GameSubm__89593A558AF23669");

            entity.ToTable("GameSubmissionImage");

            entity.Property(e => e.ImageUrl).HasMaxLength(255);

            entity.HasOne(d => d.Submission).WithMany(p => p.GameSubmissionImages)
                .HasForeignKey(d => d.SubmissionId)
                .HasConstraintName("FK__GameSubmi__Submi__6C190EBB");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF37B258881");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.LastModifiedByUser).WithMany(p => p.News)
                .HasForeignKey(d => d.LastModifiedByUserId)
                .HasConstraintName("FK__News__LastModifi__1F98B2C1");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E1268A8DE4D");

            entity.ToTable("Notification");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.NotificationType).HasMaxLength(50);
            entity.Property(e => e.ReferenceType).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__19DFD96B");
        });

        modelBuilder.Entity<NotificationPreference>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Notifica__1788CC4CAB1BC577");

            entity.ToTable("NotificationPreference");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.FeedActivityNotificationsEnabled).HasDefaultValue(true);
            entity.Property(e => e.FollowNotificationsEnabled).HasDefaultValue(true);
            entity.Property(e => e.ReviewLikeNotificationsEnabled).HasDefaultValue(true);
            entity.Property(e => e.SubmissionNotificationsEnabled).HasDefaultValue(true);

            entity.HasOne(d => d.User).WithOne(p => p.NotificationPreference)
                .HasForeignKey<NotificationPreference>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__49C3F6B7");
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__Password__658FEEEAF6D94847");

            entity.ToTable("PasswordResetToken");

            entity.HasIndex(e => e.Token, "UQ__Password__1EB4F817CC007D79").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.PasswordResetTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PasswordR__UserI__44FF419A");
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.PlatformId).HasName("PK__Platform__F559F6FAF16C291D");

            entity.ToTable("Platform");

            entity.HasIndex(e => e.Name, "UQ__Platform__737584F60057FD53").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79CEB29DE967");

            entity.ToTable("Review");

            entity.HasIndex(e => new { e.UserId, e.GameId }, "UQ__Review__D52345324EB1F48D").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Game).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__GameId__05D8E0BE");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__UserId__04E4BC85");
        });

        modelBuilder.Entity<ReviewLike>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ReviewId }).HasName("PK__ReviewLi__E0C30BD0BA78ACFE");

            entity.ToTable("ReviewLike");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Review).WithMany(p => p.ReviewLikes)
                .HasForeignKey(d => d.ReviewId)
                .HasConstraintName("FK__ReviewLik__Revie__0C85DE4D");

            entity.HasOne(d => d.User).WithMany(p => p.ReviewLikes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReviewLik__UserI__0B91BA14");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1AD0B262E1");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C7FF18BF1");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E441F0675C").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534757BF104").IsUnique();

            entity.Property(e => e.Bio).HasMaxLength(500);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.ProfileImageUrl).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasMany(d => d.Platforms).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserPreferredPlatform",
                    r => r.HasOne<Platform>().WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserPrefe__Platf__76969D2E"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__UserPrefe__UserI__75A278F5"),
                    j =>
                    {
                        j.HasKey("UserId", "PlatformId").HasName("PK__UserPref__A8DD53236734F53E");
                        j.ToTable("UserPreferredPlatform");
                    });

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRole__RoleId__412EB0B6"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRole__UserId__403A8C7D"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760AD874E8BE4");
                        j.ToTable("UserRole");
                    });
        });

        modelBuilder.Entity<UserGame>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.GameId }).HasName("PK__UserGame__D523453302AFDB28");

            entity.ToTable("UserGame");

            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PlaytimeHours).HasColumnType("decimal(6, 1)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Game).WithMany(p => p.UserGames)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserGame__GameId__7D439ABD");

            entity.HasOne(d => d.Status).WithMany(p => p.UserGames)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserGame__Status__7E37BEF6");

            entity.HasOne(d => d.User).WithMany(p => p.UserGames)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserGame__UserId__7C4F7684");
        });

        modelBuilder.Entity<WebResource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK__WebResou__4ED1816F1F64CFCC");

            entity.ToTable("WebResource");

            entity.HasIndex(e => e.Title, "UQ__WebResou__2CB664DC6AE18BFC").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.LastModifiedByUser).WithMany(p => p.WebResources)
                .HasForeignKey(d => d.LastModifiedByUserId)
                .HasConstraintName("FK__WebResour__LastM__25518C17");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
