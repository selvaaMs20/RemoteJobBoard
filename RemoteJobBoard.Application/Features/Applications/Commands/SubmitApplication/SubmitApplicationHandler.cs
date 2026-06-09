// SubmitApplicationHandler.cs
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Application;
using RemoteJobBoard.Core.Entities;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Applications.Commands.SubmitApplication;

public class SubmitApplicationHandler
    : IRequestHandler<SubmitApplicationCommand, ApplicationDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SubmitApplicationHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApplicationDto> Handle(
        SubmitApplicationCommand request,
        CancellationToken cancellationToken)
    {
        // Check job post exists and is active
        var jobPost = await _context.JobPosts
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == request.JobPostId, cancellationToken);

        if (jobPost is null)
            throw new NotFoundException("JobPost", request.JobPostId);

        if (!jobPost.IsActive)
            throw new ConflictException("This job post is no longer accepting applications.");

        // Check expiry
        if (jobPost.ExpiresAt.HasValue && jobPost.ExpiresAt < DateTime.UtcNow)
            throw new ConflictException("This job post has expired.");

        // Get job seeker profile
        var profile = await _context.JobSeekerProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
            throw new NotFoundException(
                "JobSeeker profile not found. Please complete your profile first.");

        // Check not already applied
        var alreadyApplied = await _context.Applications
            .AnyAsync(a =>
                a.JobPostId == request.JobPostId &&
                a.JobSeekerProfileId == profile.Id,
                cancellationToken);

        if (alreadyApplied)
            throw new ConflictException(
                "You have already applied for this job.");

        var application = new Core.Entities.Application
        {
            JobPostId = request.JobPostId,
            JobSeekerProfileId = profile.Id,
            CoverLetter = request.CoverLetter,
            Status = Core.Enums.ApplicationStatus.Submitted
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload with full navigation for mapping
        var created = await _context.Applications
            .Include(a => a.JobPost)
                .ThenInclude(j => j.Company)
            .Include(a => a.JobSeekerProfile)
                .ThenInclude(p => p.User)
            .FirstAsync(a => a.Id == application.Id, cancellationToken);

        return _mapper.Map<ApplicationDto>(created);
    }
}