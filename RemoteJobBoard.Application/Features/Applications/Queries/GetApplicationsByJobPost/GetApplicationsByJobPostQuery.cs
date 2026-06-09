using MediatR;
using RemoteJobBoard.Application.DTOs.Application;
using RemoteJobBoard.Application.DTOs.Common;

namespace RemoteJobBoard.Application.Features.Applications.Queries.GetApplicationsByJobPost;

public record GetApplicationsByJobPostQuery(
    Guid JobPostId,
    Guid RecruiterId,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResultDto<ApplicationDto>>;