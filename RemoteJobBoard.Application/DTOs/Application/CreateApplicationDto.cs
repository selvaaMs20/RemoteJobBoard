using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Application.DTOs.Application
{
    public class CreateApplicationDto
    {
        public Guid JobPostId { get; set; }
        public string? CoverLetter { get; set; }
    }
}
