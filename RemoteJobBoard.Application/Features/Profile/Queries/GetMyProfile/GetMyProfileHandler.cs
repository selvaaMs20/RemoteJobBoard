using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.JobseekerProfile;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Profile.Queries.GetMyProfile;

public class GetMyProfileHandler
    : IRequestHandler<GetMyProfileQuery, JobSeekerProfileDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetMyProfileHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JobSeekerProfileDto> Handle(
        GetMyProfileQuery request,
        CancellationToken cancellationToken)
    {
        var profile = await _context.JobSeekerProfiles
            .Include(p => p.User)
            .Include(p => p.Skills)
                .ThenInclude(s => s.Skill)
            .FirstOrDefaultAsync(
                p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
            throw new NotFoundException(
                "Profile not found.");

        return _mapper.Map<JobSeekerProfileDto>(profile);
    }
}