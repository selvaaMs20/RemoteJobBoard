// GetApplicationsByJobPostHandler.cs
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Application;
using RemoteJobBoard.Application.DTOs.Common;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Applications.Queries.GetApplicationsByJobPost;

public class GetApplicationsByJobPostHandler
    : IRequestHandler<GetApplicationsByJobPostQuery, PagedResultDto<ApplicationDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetApplicationsByJobPostHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<ApplicationDto>> Handle(
        GetApplicationsByJobPostQuery request,
        CancellationToken cancellationToken)
    {
        // Verify job post exists
        var jobPost = await _context.JobPosts
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == request.JobPostId, cancellationToken);

        if (jobPost is null)
            throw new NotFoundException("JobPost", request.JobPostId);

        // Verify recruiter owns this job post
        if (jobPost.Company.OwnerId != request.RecruiterId)
            throw new ForbiddenException(
                "You can only view applications for your own job posts.");

        var query = _context.Applications
            .Include(a => a.JobPost)
                .ThenInclude(j => j.Company)
            .Include(a => a.JobSeekerProfile)
                .ThenInclude(p => p.User)
            .Where(a => a.JobPostId == request.JobPostId)
            .OrderByDescending(a => a.CreatedAt)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResultDto<ApplicationDto>
        {
            Items = _mapper.Map<List<ApplicationDto>>(items),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}