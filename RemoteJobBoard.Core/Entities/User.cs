using RemoteJobBoard.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteJobBoard.Core.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.JobSeeker;
        public string? AvatarUrl { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public string? EmailVerificationToken { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        public JobSeekerProfile? JobSeekerProfile { get; set; }
        public ICollection<Company> OwnedCompanies { get; set; } = new List<Company>();
        public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
        //public ICollection<CompanyReview> Reviews { get; set; } = new List<CompanyReview>();
        //public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
