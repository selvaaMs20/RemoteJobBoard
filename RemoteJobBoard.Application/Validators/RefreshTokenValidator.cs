using FluentValidation;
using RemoteJobBoard.Application.DTOs.Auth;

namespace RemoteJobBoard.Application.Validators;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenDto>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}