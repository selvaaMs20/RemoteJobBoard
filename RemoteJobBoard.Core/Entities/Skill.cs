using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteJobBoard.Core.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }

        // Navigation
        public ICollection<JobSeekerSkill> JobSeekerSkills { get; set; } = new List<JobSeekerSkill>();
        public ICollection<JobPostSkill> JobPostSkills { get; set; } = new List<JobPostSkill>();
    }
}
