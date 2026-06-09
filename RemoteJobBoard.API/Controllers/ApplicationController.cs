using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteJobBoard.Application.DTOs.Application;
using RemoteJobBoard.Application.Features.Applications.Commands.SubmitApplication;
using RemoteJobBoard.Application.Features.Applications.Commands.UpdateApplicationStatus;
using RemoteJobBoard.Application.Features.Applications.Queries.GetApplicationsByJobPost;
using RemoteJobBoard.Application.Features.Applications.Queries.GetMyApplications;
using RemoteJobBoard.Core.Enums;
using System.Security.Claims;

namespace RemoteJobBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Job seeker submits application
    [HttpPost]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> Submit([FromBody] CreateApplicationDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new SubmitApplicationCommand(
            dto.JobPostId,
            userId,
            dto.CoverLetter
        ));

        return Ok(result);
    }

    // Job seeker views their own applications
    [HttpGet("my")]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> GetMyApplications(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(
            new GetMyApplicationsQuery(userId, pageNumber, pageSize));

        return Ok(result);
    }

    // Recruiter views all applications for a job post
    [HttpGet("jobpost/{jobPostId:guid}")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> GetByJobPost(
        Guid jobPostId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var recruiterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new GetApplicationsByJobPostQuery(
            jobPostId, recruiterId, pageNumber, pageSize));

        return Ok(result);
    }

    // Recruiter updates application status
    [HttpPatch("{applicationId:guid}/status")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> UpdateStatus(
        Guid applicationId,
        [FromBody] UpdateApplicationStatusDto dto)
    {
        var recruiterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new UpdateApplicationStatusCommand(
            applicationId,
            recruiterId,
            dto.Status
        ));

        return Ok(result);
    }
}