using RemoteJobBoard.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Core.Entities
{
    public class Application : BaseEntity
    {
        public Guid JobPostId { get; set; }
        public Guid JobSeekerProfileId { get; set; }
        public string? CoverLetter { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

        // Navigation
        public JobPost JobPost { get; set; } = null!;
        public JobSeekerProfile JobSeekerProfile { get; set; } = null!;
        //public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
