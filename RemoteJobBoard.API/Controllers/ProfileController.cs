using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteJobBoard.Application.DTOs.JobseekerProfile;
using RemoteJobBoard.Application.Features.Profile.Commands.UpdateProfile;
using RemoteJobBoard.Application.Features.Profile.Queries.GetMyProfile;
using System.Security.Claims;

namespace RemoteJobBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "JobSeeker")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(
            new GetMyProfileQuery(userId));
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateJobSeekerProfileDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(
            new UpdateProfileCommand(userId, dto));
        return Ok(result);
    }
}