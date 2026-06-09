using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Application;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Applications.Commands.UpdateApplicationStatus;

public class UpdateApplicationStatusHandler
    : IRequestHandler<UpdateApplicationStatusCommand, ApplicationDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UpdateApplicationStatusHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApplicationDto> Handle(
        UpdateApplicationStatusCommand request,
        CancellationToken cancellationToken)
    {
        var application = await _context.Applications
            .Include(a => a.JobPost)
                .ThenInclude(j => j.Company)
            .Include(a => a.JobSeekerProfile)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(
                a => a.Id == request.ApplicationId, cancellationToken);

        if (application is null)
            throw new NotFoundException("Application", request.ApplicationId);

        // Verify recruiter owns the company that posted this job
        var isOwner = application.JobPost.Company.OwnerId == request.RecruiterId;
        if (!isOwner)
            throw new ForbiddenException(
                "You can only update applications for your own job posts.");

        application.Status = request.NewStatus;
        application.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ApplicationDto>(application);
    }
}