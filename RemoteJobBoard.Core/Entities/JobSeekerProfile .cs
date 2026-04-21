// JobSeekerProfile.cs
using static System.Net.Mime.MediaTypeNames;

namespace RemoteJobBoard.Core.Entities;

public class JobSeekerProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public string? Title { get; set; }
    public string? Bio { get; set; }
    public string? ResumeUrl { get; set; }
    public string? Location { get; set; }
    public int YearsOfExperience { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public bool OpenToWork { get; set; } = true;

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<JobSeekerSkill> Skills { get; set; } = new List<JobSeekerSkill>();
    public ICollection<Application> Applications { get; set; } = new List<Application>();


}