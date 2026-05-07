using GameVault.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Data;

public class GameVaultContext : DbContext
{
    public GameVaultContext(DbContextOptions<GameVaultContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameImage> GameImages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<WebResource> WebResources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
        });

        // Game entity configuration
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.CoverArtUrl).IsRequired();
            entity.Property(e => e.Developer).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Publisher).IsRequired().HasMaxLength(255);
        });

        // GameImage entity configuration
        modelBuilder.Entity<GameImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);
            entity.HasOne(e => e.Game)
                .WithMany(g => g.GameImages)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Category entity configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        });

        // Platform entity configuration
        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.PlatformId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        });

        // GameCategory junction table (many-to-many)
        modelBuilder.Entity<Game>()
            .HasMany(g => g.Categories)
            .WithMany(c => c.Games)
            .UsingEntity<Dictionary<string, object>>(
                "GameCategory",
                j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("GameId", "CategoryId");
                    j.ToTable("GameCategory");
                }
            );

        // GamePlatform junction table (many-to-many)
        modelBuilder.Entity<Game>()
            .HasMany(g => g.Platforms)
            .WithMany(p => p.Games)
            .UsingEntity<Dictionary<string, object>>(
                "GamePlatform",
                j => j.HasOne<Platform>().WithMany().HasForeignKey("PlatformId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Game>().WithMany().HasForeignKey("GameId").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("GameId", "PlatformId");
                    j.ToTable("GamePlatform");
                }
            );

        // Review entity configuration
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId);
            entity.HasOne(e => e.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // UNIQUE constraint on (UserId, GameId)
            entity.HasIndex(e => new { e.UserId, e.GameId }).IsUnique();
        });

        // News entity configuration
        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Content).IsRequired();
            entity.HasOne(e => e.LastModifiedByUser)
                .WithMany(u => u.ModifiedNews)
                .HasForeignKey(e => e.LastModifiedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // WebResource entity configuration
        modelBuilder.Entity<WebResource>(entity =>
        {
            entity.HasKey(e => e.ResourceId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.HtmlContent).IsRequired();
            entity.HasOne(e => e.LastModifiedByUser)
                .WithMany(u => u.ModifiedWebResources)
                .HasForeignKey(e => e.LastModifiedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
