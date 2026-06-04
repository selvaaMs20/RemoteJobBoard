
using MediatR;
using RemoteJobBoard.Application.DTOs.JobPost;
using RemoteJobBoard.Core.Enums;

namespace RemoteJobBoard.Application.Features.JobPosts.Commands.CreateJobPost;

public record CreateJobPostCommand(
    Guid CompanyId,
    string Title,
    string Description,
    JobType JobType,
    WorkMode WorkMode,
    ExperienceLevel ExperienceLevel,
    decimal? SalaryMin,
    decimal? SalaryMax,
    string Currency,
    DateTime? ExpiresAt,
    List<Guid> RequiredSkillIds
) : IRequest<JobPostDto>;