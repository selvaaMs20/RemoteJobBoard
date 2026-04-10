// JobSeekerSkill.cs (junction table)
using RemoteJobBoard.Core.Enums;

namespace RemoteJobBoard.Core.Entities;

public class JobSeekerSkill
{
    public Guid JobSeekerProfileId { get; set; }
    public Guid SkillId { get; set; }
    public ProficiencyLevel ProficiencyLevel { get; set; } = ProficiencyLevel.Intermediate;

    // Navigation
    public JobSeekerProfile JobSeekerProfile { get; set; } = null!;
    public Skill Skill { get; set; } = null!;
}