using MediatR;
using RemoteJobBoard.Application.DTOs.Auth;

namespace RemoteJobBoard.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;