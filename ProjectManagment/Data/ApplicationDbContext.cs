using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using ProjectManagment.Areas.Identity.Data;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace ProjectManagment.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Status> Status { get; set; }
    public DbSet<LabelsToIssues> LabelsToIssues { get; set; }
    public DbSet<UsersToIssues> UsersToIssues { get; set; }
    public DbSet<ProjectsToMembers> ProjectsToMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Project>()
        .HasMany(e => e.Issues)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .OnDelete(DeleteBehavior.NoAction)
        .HasPrincipalKey(e => e.Id);

        builder.Entity<Project>()
        .HasMany(e => e.Milestones)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .OnDelete(DeleteBehavior.NoAction)
        .HasPrincipalKey(e => e.Id);

         builder.Entity<Project>()
        .HasMany(e => e.Labels)
        .WithOne(e => e.Project)
        .HasForeignKey(e => e.ProjectId)
        .OnDelete(DeleteBehavior.NoAction)
        .HasPrincipalKey(e => e.Id);

        builder.Entity<Project>()
      .HasMany(e => e.Statuses)
      .WithOne(e => e.Project)
      .HasForeignKey(e => e.ProjectId)
      .OnDelete(DeleteBehavior.NoAction)
      .HasPrincipalKey(e => e.Id);

        builder.Entity<ApplicationUser>()
        .HasMany(e => e.ProjectsJoined)
        .WithOne(e => e.Owner)
        .HasForeignKey(e => e.OwnerId)
        .OnDelete(DeleteBehavior.NoAction)
        .HasPrincipalKey(e => e.Id);

        builder.Entity<ApplicationUser>()
        .HasMany(e => e.Issues)
        .WithOne(e => e.Owner)
        .HasForeignKey(e => e.OwnerId)
        .OnDelete(DeleteBehavior.NoAction)
        .HasPrincipalKey(e => e.Id);

        builder.Entity<ApplicationUser>()
       .HasMany(e => e.Comments)
       .WithOne(e => e.Author)
       .HasForeignKey(e => e.AuthorId)
       .OnDelete(DeleteBehavior.NoAction)
       .HasPrincipalKey(e => e.Id);

        builder.Entity<Issue>()
       .HasMany(e => e.Comments)
       .WithOne(e => e.Issue)
       .OnDelete(DeleteBehavior.NoAction)
       .HasForeignKey(e => e.IssueId)
       .OnDelete(DeleteBehavior.NoAction)
        .HasPrincipalKey(e => e.Id);

        builder.Entity<Issue>()
       .HasMany(e => e.Assignees)
       .WithMany(e => e.IssuesAssigned);
       

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        //builder.Property(u => u.UserName).HasMaxLength(255)
        //    .IsRequired();
    }
}
