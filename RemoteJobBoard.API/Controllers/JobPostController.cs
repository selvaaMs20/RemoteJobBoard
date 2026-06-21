using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteJobBoard.Application.DTOs.JobPost;
using RemoteJobBoard.Application.Features.JobPosts.Commands.CreateJobPost;
using RemoteJobBoard.Application.Features.JobPosts.Commands.DeleteJobPost;
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

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? workMode = null,
        [FromQuery] string? jobType = null,
        [FromQuery] string? experienceLevel = null,
        [FromQuery] decimal? minSalary = null,
        [FromQuery] decimal? maxSalary = null)
    {
        var result = await _mediator.Send(new GetAllJobPostsQuery(
            pageNumber, pageSize, search,
            workMode, jobType, experienceLevel,
            minSalary, maxSalary));
       
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetJobPostByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Recruiter")]
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

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Recruiter,Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteJobPostCommand(id));
        return NoContent();
    }
}