using MediatR;
using RemoteJobBoard.Application.DTOs.JobPost;

namespace RemoteJobBoard.Application.Features.JobPosts.Queries.GetJobPostById;

public record GetJobPostByIdQuery(Guid Id) : IRequest<JobPostDto>;