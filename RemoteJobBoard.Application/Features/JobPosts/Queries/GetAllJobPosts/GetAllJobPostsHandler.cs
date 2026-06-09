using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Common;
using RemoteJobBoard.Application.DTOs.JobPost;
using RemoteJobBoard.Core.Interfaces;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.JobPosts.Queries.GetAllJobPosts;

public class GetAllJobPostsHandler
    : IRequestHandler<GetAllJobPostsQuery, PagedResultDto<JobPostDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public GetAllJobPostsHandler(
        AppDbContext context,
        IMapper mapper,
        ICacheService cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<PagedResultDto<JobPostDto>> Handle(
        GetAllJobPostsQuery request,
        CancellationToken cancellationToken)
    {
        // Only cache unfiltered first-page requests
        // Filtered/searched results are too dynamic to cache
        var isDefaultQuery =
            request.PageNumber == 1 &&
            request.Search is null &&
            request.WorkMode is null &&
            request.JobType is null &&
            request.ExperienceLevel is null &&
            request.MinSalary is null &&
            request.MaxSalary is null;

        var cacheKey = $"jobposts:page:{request.PageNumber}:size:{request.PageSize}";

        if (isDefaultQuery)
        {
            var cached = await _cache.GetAsync<PagedResultDto<JobPostDto>>(cacheKey);
            if (cached is not null)
            {
                return cached;
            }
        }

        // Build base query
        var query = _context.JobPosts
            .Include(j => j.Company)
            .Include(j => j.RequiredSkills)
                .ThenInclude(s => s.Skill)
            .Where(j => j.IsActive)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(j =>
                j.Title.ToLower().Contains(search) ||
                j.Description.ToLower().Contains(search) ||
                j.Company.Name.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(request.WorkMode) &&
            Enum.TryParse<Core.Enums.WorkMode>(request.WorkMode, true, out var workMode))
        {
            query = query.Where(j => j.WorkMode == workMode);
        }

        if (!string.IsNullOrWhiteSpace(request.JobType) &&
            Enum.TryParse<Core.Enums.JobType>(request.JobType, true, out var jobType))
        {
            query = query.Where(j => j.JobType == jobType);
        }

        if (!string.IsNullOrWhiteSpace(request.ExperienceLevel) &&
            Enum.TryParse<Core.Enums.ExperienceLevel>(
                request.ExperienceLevel, true, out var expLevel))
        {
            query = query.Where(j => j.ExperienceLevel == expLevel);
        }

        if (request.MinSalary.HasValue)
        {
            query = query.Where(j =>
                j.SalaryMin >= request.MinSalary.Value ||
                j.SalaryMax >= request.MinSalary.Value);
        }

        if (request.MaxSalary.HasValue)
        {
            query = query.Where(j =>
                j.SalaryMax <= request.MaxSalary.Value);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination + ordering
        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var result = new PagedResultDto<JobPostDto>
        {
            Items = _mapper.Map<List<JobPostDto>>(items),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        // Cache only default unfiltered queries
        if (isDefaultQuery)
        {
            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        }

        return result;
    }
}