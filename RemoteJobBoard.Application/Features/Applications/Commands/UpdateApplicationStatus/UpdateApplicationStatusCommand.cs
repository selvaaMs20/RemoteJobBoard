// UpdateApplicationStatusCommand.cs
using MediatR;
using RemoteJobBoard.Application.DTOs.Application;
using RemoteJobBoard.Core.Enums;

namespace RemoteJobBoard.Application.Features.Applications.Commands.UpdateApplicationStatus;

public record UpdateApplicationStatusCommand(
    Guid ApplicationId,
    Guid RecruiterId,
    ApplicationStatus NewStatus
) : IRequest<ApplicationDto>;