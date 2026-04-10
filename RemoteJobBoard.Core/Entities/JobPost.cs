using RemoteJobBoard.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteJobBoard.Core.Entities
{
    public class JobPost : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public JobType JobType { get; set; }
        public WorkMode WorkMode { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string Currency { get; set; } = "USD";
        public bool IsActive { get; set; } = true;
        public DateTime? ExpiresAt { get; set; }

        // Navigation
        //public Company Company { get; set; } = null!;
        public ICollection<JobPostSkill> RequiredSkills { get; set; } = new List<JobPostSkill>();
        //public ICollection<Application> Applications { get; set; } = new List<Application>();
        //public ICollection<SavedJob> SavedByUsers { get; set; } = new List<SavedJob>();
    }
}
