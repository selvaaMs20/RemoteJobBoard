// Auth/Commands/Register/RegisterHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoteJobBoard.Application.DTOs.Auth;
using RemoteJobBoard.Core.Entities;
using RemoteJobBoard.Core.Enums;
using RemoteJobBoard.Core.Exceptions;
using RemoteJobBoard.Core.Interfaces;
using RemoteJobBoard.Infrastructure.Data;

namespace RemoteJobBoard.Application.Features.Auth.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public RegisterHandler(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (exists)
            throw new ConflictException("A user with this email already exists.");

        var role = Enum.Parse<UserRole>(request.Role, ignoreCase: true);

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = role,
            RefreshToken = _jwtService.GenerateRefreshToken(),
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
        };

        // Create JobSeekerProfile automatically for job seekers
        if (role == UserRole.JobSeeker)
        {
            user.JobSeekerProfile = new JobSeekerProfile
            {
                UserId = user.Id
            };
        }

        _context.Users.Add(user);
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