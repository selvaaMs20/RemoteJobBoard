using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteJobBoard.Core.Entities
{
    public class JobPostSkill
    {
        public int Id { get; set; }
        public Guid JobPostId { get; set; }
        public Guid SkillId { get; set; }
        public bool IsRequired { get; set; } = true;

        // Navigation
        public JobPost JobPost { get; set; } = null!;
        public Skill Skill { get; set; } = null!;
    }
}
