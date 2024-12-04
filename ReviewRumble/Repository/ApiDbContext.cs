using Microsoft.EntityFrameworkCore;
using ReviewRumble.Models;

namespace ReviewRumble.Repository;

public class ApiDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<PullRequest> PullRequests { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PullRequest>()
            .HasOne(p => p.PrimaryReviewer)
            .WithMany(u => u.AssignedPullRequests)
            .HasForeignKey(p => p.PrimaryReviewerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_PullRequest_PrimaryReviewer");

        modelBuilder.Entity<PullRequest>()
            .HasOne(p => p.SecondaryReviewer)
            .WithMany(u => u.AssignedPullRequests)
            .HasForeignKey(p => p.SecondaryReviewerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_PullRequest_SecondaryReviewer");

        modelBuilder.Entity<PullRequest>()
            .HasOne(p => p.Author)
            .WithMany(u => u.MyPullRequests)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_PullRequest_Author");

        base.OnModelCreating(modelBuilder);
    }
}