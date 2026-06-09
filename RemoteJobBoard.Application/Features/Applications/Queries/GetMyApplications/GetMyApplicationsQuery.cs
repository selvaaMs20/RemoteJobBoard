
using MediatR;
using RemoteJobBoard.Application.DTOs.Application;
using RemoteJobBoard.Application.DTOs.Common;

namespace RemoteJobBoard.Application.Features.Applications.Queries.GetMyApplications;

public record GetMyApplicationsQuery(
    Guid UserId,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResultDto<ApplicationDto>>;