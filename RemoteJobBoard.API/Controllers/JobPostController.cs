// JobPostController.cs
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RemoteJobBoard.Application.DTOs.JobPost;
using RemoteJobBoard.Application.Features.JobPosts.Commands.CreateJobPost;
using RemoteJobBoard.Application.Features.JobPosts.Queries.GetAllJobPosts;
using RemoteJobBoard.Application.Features.JobPosts.Queries.GetJobPostById;

namespace RemoteJobBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobPostController : ControllerBase
{
    private readonly IMediator _mediator;
    public JobPostController(IMediator mediator)
    {
        _mediator = mediator;
    }
    private readonly IMapper _mapper;

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(
            new GetAllJobPostsQuery(pageNumber, pageSize));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetJobPostByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobPostDto dto)
    {
        var result = await _mediator.Send(new CreateJobPostCommand(
            dto.CompanyId,
            dto.Title,
            dto.Description,
            dto.JobType,
            dto.WorkMode,
            dto.ExperienceLevel,
            dto.SalaryMin,
            dto.SalaryMax,
            dto.Currency,
            dto.ExpiresAt,
            dto.RequiredSkillIds
        ));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}