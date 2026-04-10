// AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Core.Entities;

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
            .Property(j => j.SalaryMax);
    }
}