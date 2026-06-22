using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Skill;
using RemoteJobBoard.Infrastructure.Data;
using AutoMapper;

namespace RemoteJobBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SkillsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var skills = await _context.Skills
            .OrderBy(s => s.Name)
            .ToListAsync();
        return Ok(_mapper.Map<List<SkillDto>>(skills));
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        if (await _context.Skills.AnyAsync())
            return Ok("Skills already seeded.");

        var skills = new[]
        {
            "Flutter", ".NET Core", "C#", "Azure",
            "Firebase", "SQL Server", "Redis", "Docker",
            "React", "Node.js", "Python", "Java",
            "TypeScript", "JavaScript", "Swift", "Kotlin",
            "PostgreSQL", "MongoDB", "GraphQL", "REST APIs",
            "Git", "CI/CD", "Kubernetes", "AWS"
        }.Select(name => new Core.Entities.Skill
        {
            Name = name,
            Category = "Technology"
        });

        _context.Skills.AddRange(skills);
        await _context.SaveChangesAsync();
        return Ok("Skills seeded successfully.");
    }
}