// AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Core.Entities;
using RemoteJobBoard.Infrastructure.Migrations;

namespace RemoteJobBoard.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<JobSeekerProfile> JobSeekerProfiles => Set<JobSeekerProfile>();
    public DbSet<JobSeekerSkill> JobSeekerSkills => Set<JobSeekerSkill>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<JobPostSkill> JobPostSkills => Set<JobPostSkill>();
    public DbSet<JobPost> JobPosts => Set<JobPost>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<SavedJob> SavedJobs => Set<SavedJob>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<ChatMessage> Messages => Set<ChatMessage>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<CompanyReview> CompanyReviews => Set<CompanyReview>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<JobSeekerSkill>()
           .HasKey(x => new { x.JobSeekerProfileId, x.SkillId });

        modelBuilder.Entity<JobPostSkill>()
            .HasKey(x => new { x.JobPostId, x.SkillId });

        // Decimal precision for salary
        modelBuilder.Entity<JobPost>()
            .Property(j => j.SalaryMin)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<JobPost>()
            .Property(j => j.SalaryMax)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<ChatMessage>()
             .HasOne(m => m.Sender)
             .WithMany()
             .HasForeignKey(m => m.SenderId)
             .OnDelete(DeleteBehavior.NoAction); // IMPORTANT

        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.NoAction); // IMPORTANT

        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Application)
            .WithMany()
            .HasForeignKey(m => m.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade); // this is fine


        modelBuilder.Entity<Company>()
            .HasOne(c => c.Owner)
            .WithMany()                        // or WithMany(u => u.Companies)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}