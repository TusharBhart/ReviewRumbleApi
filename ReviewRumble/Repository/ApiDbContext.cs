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
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<PullRequestReviewer>()
            .HasKey(prr => new { prr.PullRequestId, prr.ReviewerId });

        modelBuilder.Entity<PullRequestReviewer>()
            .HasOne(prr => prr.PullRequest)
            .WithMany(pr => pr.Reviewers)
            .HasForeignKey(prr => prr.PullRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PullRequestReviewer>()
            .HasOne(prr => prr.Reviewer)
            .WithMany(u => u.AssignedReviews)
            .HasForeignKey(prr => prr.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PullRequest>()
            .HasOne(prr => prr.Author)
            .WithMany(u => u.MyPullRequests)
            .HasForeignKey(prr => prr.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}