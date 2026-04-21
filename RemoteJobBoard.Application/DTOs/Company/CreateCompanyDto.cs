using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteJobBoard.Application.DTOs.Company
{
    public class CreateCompanyDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? Location { get; set; }
        public string? Industry { get; set; }
        public string? Description { get; set; }
    }
}
