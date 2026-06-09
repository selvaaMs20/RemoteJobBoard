using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Application;
using RemoteJobBoard.Application.DTOs.Common;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Applications.Queries.GetMyApplications;

public class GetMyApplicationsHandler
    : IRequestHandler<GetMyApplicationsQuery, PagedResultDto<ApplicationDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetMyApplicationsHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<ApplicationDto>> Handle(
        GetMyApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        // Get job seeker profile
        var profile = await _context.JobSeekerProfiles
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
            throw new NotFoundException(
                "JobSeeker profile not found. Please complete your profile first.");

        var query = _context.Applications
            .Include(a => a.JobPost)
                .ThenInclude(j => j.Company)
            .Include(a => a.JobSeekerProfile)
                .ThenInclude(p => p.User)
            .Where(a => a.JobSeekerProfileId == profile.Id)
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