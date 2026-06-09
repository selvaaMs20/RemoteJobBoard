using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Core.Interfaces;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.JobPosts.Commands.DeleteJobPost;

public class DeleteJobPostHandler : IRequestHandler<DeleteJobPostCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;

    public DeleteJobPostHandler(AppDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<bool> Handle(
        DeleteJobPostCommand request,
        CancellationToken cancellationToken)
    {
        var jobPost = await _context.JobPosts
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken);

        if (jobPost is null)
            throw new NotFoundException("JobPost", request.Id);

        _context.JobPosts.Remove(jobPost);
        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate cache
        await _cache.RemoveAsync("jobposts:page:1:size:10");

        return true;
    }
}