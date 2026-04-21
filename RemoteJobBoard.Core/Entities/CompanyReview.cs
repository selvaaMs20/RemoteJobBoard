using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Core.Entities
{
    public class CompanyReview : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string? ReviewText { get; set; }

        // Navigation
        public Company Company { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
