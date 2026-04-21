using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Core.Entities
{
    public class SavedJob : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid JobPostId { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public JobPost JobPost { get; set; } = null!;
    }
}
