using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Application.DTOs.Company
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public string? Location { get; set; }
        public string? Industry { get; set; }
        public string? Description { get; set; }
        public bool IsVerified { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
