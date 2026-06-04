// GetAllJobPostsHandler.cs
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Common;
using RemoteJobBoard.Application.DTOs.JobPost;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.JobPosts.Queries.GetAllJobPosts;

public class GetAllJobPostsHandler
    : IRequestHandler<GetAllJobPostsQuery, PagedResultDto<JobPostDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllJobPostsHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<JobPostDto>> Handle(
        GetAllJobPostsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.JobPosts
            .Include(j => j.Company)
            .Include(j => j.RequiredSkills)
                .ThenInclude(s => s.Skill)
            .Where(j => j.IsActive)
            .OrderByDescending(j => j.CreatedAt)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResultDto<JobPostDto>
        {
            Items = _mapper.Map<List<JobPostDto>>(items),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}