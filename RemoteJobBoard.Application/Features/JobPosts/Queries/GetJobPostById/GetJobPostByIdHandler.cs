
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.JobPost;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.JobPosts.Queries.GetJobPostById;

public class GetJobPostByIdHandler : IRequestHandler<GetJobPostByIdQuery, JobPostDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetJobPostByIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JobPostDto> Handle(
        GetJobPostByIdQuery request,
        CancellationToken cancellationToken)
    {
        var jobPost = await _context.JobPosts
            .Include(j => j.Company)
            .Include(j => j.RequiredSkills)
                .ThenInclude(s => s.Skill)
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken);

        if (jobPost is null)
            throw new NotFoundException("JobPost", request.Id);

        return _mapper.Map<JobPostDto>(jobPost);
    }
}