// CompanyController.cs
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RemoteJobBoard.Application.DTOs.Company;
using RemoteJobBoard.Application.Features.Companies.Commands.CreateCompany;
using RemoteJobBoard.Application.Features.Companies.Queries.GetCompanyById;
using System.Security.Claims;

namespace RemoteJobBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
    {
        // Get logged-in user's ID from JWT claims
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new CreateCompanyCommand(
            userId,
            dto.Name,
            dto.Website,
            dto.Location,
            dto.Industry,
            dto.Description
        ));

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCompanyByIdQuery(id));
        return Ok(result);
    }
}