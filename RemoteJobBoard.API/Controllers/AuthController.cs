using MediatR;
using Microsoft.AspNetCore.Mvc;
using RemoteJobBoard.Application.DTOs.Auth;
using RemoteJobBoard.Application.Features.Auth.Commands.Login;
using RemoteJobBoard.Application.Features.Auth.Commands.RefreshToken;
using RemoteJobBoard.Application.Features.Auth.Commands.Register;

namespace RemoteJobBoard.API.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase    
    {
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _mediator.Send(
            new RegisterCommand(dto.Name, dto.Email, dto.Password, dto.Role));
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _mediator.Send(
            new LoginCommand(dto.Email, dto.Password));
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
    {
        var result = await _mediator.Send(
            new RefreshTokenCommand(dto.RefreshToken));
        return Ok(result);
    }
}

