// UpdateApplicationStatusDto.cs
using RemoteJobBoard.Core.Enums;

namespace RemoteJobBoard.Application.DTOs.Application;

public class UpdateApplicationStatusDto
{
    public ApplicationStatus Status { get; set; }
}