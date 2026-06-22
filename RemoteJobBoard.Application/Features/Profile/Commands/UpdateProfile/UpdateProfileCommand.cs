using MediatR;
using RemoteJobBoard.Application.DTOs.JobseekerProfile;

namespace RemoteJobBoard.Application.Features.Profile.Commands.UpdateProfile;

public record UpdateProfileCommand(
    Guid UserId,
    UpdateJobSeekerProfileDto Dto
) : IRequest<JobSeekerProfileDto>;