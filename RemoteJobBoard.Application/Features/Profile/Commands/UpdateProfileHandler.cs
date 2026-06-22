using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.JobseekerProfile;
using RemoteJobBoard.Core.Entities;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileHandler
    : IRequestHandler<UpdateProfileCommand, JobSeekerProfileDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UpdateProfileHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JobSeekerProfileDto> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var profile = await _context.JobSeekerProfiles
            .Include(p => p.User)
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(
                p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
            throw new NotFoundException("Profile not found.");

        var dto = request.Dto;
        profile.Title = dto.Title;
        profile.Bio = dto.Bio;
        profile.Location = dto.Location;
        profile.YearsOfExperience = dto.YearsOfExperience;
        profile.LinkedInUrl = dto.LinkedInUrl;
        profile.GitHubUrl = dto.GitHubUrl;
        profile.OpenToWork = dto.OpenToWork;
        profile.UpdatedAt = DateTime.UtcNow;

        // Update skills
        profile.Skills.Clear();
        if (dto.SkillIds.Any())
        {
            var skills = await _context.Skills
                .Where(s => dto.SkillIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            profile.Skills = skills.Select(s =>
                new JobSeekerSkill
                {
                    JobSeekerProfileId = profile.Id,
                    SkillId = s.Id,
                    ProficiencyLevel = Core.Enums
                        .ProficiencyLevel.Intermediate
                }).ToList();
        }

        await _context.SaveChangesAsync(cancellationToken);

        // Reload with navigation
        var updated = await _context.JobSeekerProfiles
            .Include(p => p.User)
            .Include(p => p.Skills)
                .ThenInclude(s => s.Skill)
            .FirstAsync(
                p => p.Id == profile.Id, cancellationToken);

        return _mapper.Map<JobSeekerProfileDto>(updated);
    }
}