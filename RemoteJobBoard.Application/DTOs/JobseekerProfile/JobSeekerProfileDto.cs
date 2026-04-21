using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Application.DTOs.JobseekerProfile
{
    public class JobSeekerProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? ResumeUrl { get; set; }
        public string? Location { get; set; }
        public int YearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public bool OpenToWork { get; set; }
        public List<string> Skills { get; set; } = new();
    }
}
