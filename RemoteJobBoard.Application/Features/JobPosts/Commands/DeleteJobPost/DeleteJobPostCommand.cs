using MediatR;

namespace RemoteJobBoard.Application.Features.JobPosts.Commands.DeleteJobPost;

public record DeleteJobPostCommand(Guid Id) : IRequest<bool>;