using MediatR;
using RemoteJobBoard.Application.DTOs.Auth;

namespace RemoteJobBoard.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Name,
    string Email,
    string Password,
    string Role
) : IRequest<AuthResponseDto>;