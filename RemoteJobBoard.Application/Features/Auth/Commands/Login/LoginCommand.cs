// Auth/Commands/Login/LoginCommand.cs
using MediatR;
using RemoteJobBoard.Application.DTOs.Auth;

namespace RemoteJobBoard.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;