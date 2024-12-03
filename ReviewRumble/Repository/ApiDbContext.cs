using Microsoft.EntityFrameworkCore;
using ReviewRumble.Models;
using System;

namespace ReviewRumble.Repository;

public class ApiDbContext : DbContext
{
    public DbSet<Reviewer> Reviewers { get; set; }
    public DbSet<PullRequest> PullRequests { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PullRequest>()
            .HasOne(p => p.AssignedFirstReviewer)
            .WithMany()
            .HasForeignKey(p => p.AssignedFirstReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PullRequest>()
            .HasOne(p => p.AssignedSecondReviewer)
            .WithMany()
            .HasForeignKey(p => p.AssignedSecondReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}