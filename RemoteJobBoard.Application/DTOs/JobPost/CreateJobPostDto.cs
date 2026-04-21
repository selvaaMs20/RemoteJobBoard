using RemoteJobBoard.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Application.DTOs.JobPost
{
    public class CreateJobPostDto
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
        public DateTime? ExpiresAt { get; set; }
        public List<Guid> RequiredSkillIds { get; set; } = new();
    }
}
