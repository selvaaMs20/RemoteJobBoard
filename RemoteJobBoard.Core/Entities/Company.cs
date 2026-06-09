using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Core.Entities
{
    public class Company : BaseEntity
    {
        public Guid? OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public string? Location { get; set; }
        public string? Industry { get; set; }
        public string? Description { get; set; }
        public bool IsVerified { get; set; } = false;

        // Navigation
        public User Owner { get; set; } = null!;
        public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();
        public ICollection<CompanyReview> Reviews { get; set; } = new List<CompanyReview>();
    }
}
