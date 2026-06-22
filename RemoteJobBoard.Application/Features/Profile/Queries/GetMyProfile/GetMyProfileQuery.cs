using MediatR;
using RemoteJobBoard.Application.DTOs.JobseekerProfile;

namespace RemoteJobBoard.Application.Features.Profile.Queries.GetMyProfile;

public record GetMyProfileQuery(Guid UserId)
    : IRequest<JobSeekerProfileDto>;