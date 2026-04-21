// JobPostController.cs
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RemoteJobBoard.Application.DTOs.JobPost;

namespace RemoteJobBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobPostController : ControllerBase
{
    private readonly IMapper _mapper;

    public JobPostController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateJobPostDto dto)
    {
        // Validation runs automatically before reaching here
        // If invalid, FluentValidation returns 400 with error details
        return Ok(new { message = "Valid request received", data = dto });
    }
}