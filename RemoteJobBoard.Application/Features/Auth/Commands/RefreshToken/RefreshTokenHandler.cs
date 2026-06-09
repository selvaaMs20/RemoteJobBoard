// Auth/Commands/RefreshToken/RefreshTokenHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Auth;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Core.Interfaces;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public RefreshTokenHandler(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.RefreshToken == request.RefreshToken &&
                u.RefreshTokenExpiry > DateTime.UtcNow,
                cancellationToken);

        if (user is null)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        user.RefreshToken = _jwtService.GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = _jwtService.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken!,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}