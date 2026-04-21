
using FluentValidation;
using RemoteJobBoard.Application.DTOs.Company;

namespace RemoteJobBoard.Application.Validators;

public class CreateCompanyValidator : AbstractValidator<CreateCompanyDto>
{
    public CreateCompanyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MinimumLength(2).WithMessage("Company name must be at least 2 characters.")
            .MaximumLength(150).WithMessage("Company name cannot exceed 150 characters.");

        RuleFor(x => x.Website)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Website must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.Website));

        RuleFor(x => x.Industry)
            .MaximumLength(100).WithMessage("Industry cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Industry));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}