
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.JobPost;
using RemoteJobBoard.Core.Entities;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Core.Interfaces;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.JobPosts.Commands.CreateJobPost;

public class CreateJobPostHandler : IRequestHandler<CreateJobPostCommand, JobPostDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public CreateJobPostHandler(AppDbContext context, IMapper mapper, ICacheService cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<JobPostDto> Handle(
        CreateJobPostCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);

        if (company is null)
            throw new NotFoundException("Company", request.CompanyId);

        var skills = await _context.Skills
            .Where(s => request.RequiredSkillIds.Contains(s.Id))
            .ToListAsync(cancellationToken);

        var jobPost = new JobPost
        {
            CompanyId = request.CompanyId,
            Title = request.Title,
            Description = request.Description,
            JobType = request.JobType,
            WorkMode = request.WorkMode,
            ExperienceLevel = request.ExperienceLevel,
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax,
            Currency = request.Currency,
            ExpiresAt = request.ExpiresAt,
            RequiredSkills = skills.Select(s => new JobPostSkill
            {
                SkillId = s.Id,
                IsRequired = true
            }).ToList()
        };

        _context.JobPosts.Add(jobPost);
        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate job feed cache so new job appears immediately
        await _cache.RemoveAsync("jobposts:page:1:size:10");

        var created = await _context.JobPosts
            .Include(j => j.Company)
            .Include(j => j.RequiredSkills)
                .ThenInclude(s => s.Skill)
            .FirstAsync(j => j.Id == jobPost.Id, cancellationToken);

        return _mapper.Map<JobPostDto>(created);
    }
}