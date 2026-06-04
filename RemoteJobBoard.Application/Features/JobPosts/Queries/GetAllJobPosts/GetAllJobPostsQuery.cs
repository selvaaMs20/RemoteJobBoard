using MediatR;
using RemoteJobBoard.Application.DTOs.Common;
using RemoteJobBoard.Application.DTOs.JobPost;

namespace RemoteJobBoard.Application.Features.JobPosts.Queries.GetAllJobPosts;

public record GetAllJobPostsQuery(int PageNumber = 1, int PageSize = 10)
    : IRequest<PagedResultDto<JobPostDto>>;
