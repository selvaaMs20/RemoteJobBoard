// SubmitApplicationCommand.cs
using MediatR;
using RemoteJobBoard.Application.DTOs.Application;

namespace RemoteJobBoard.Application.Features.Applications.Commands.SubmitApplication;

public record SubmitApplicationCommand(
    Guid JobPostId,
    Guid UserId,
    string? CoverLetter
) : IRequest<ApplicationDto>;