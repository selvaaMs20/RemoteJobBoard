
using FluentValidation;
using RemoteJobBoard.Application.DTOs.JobPost;

namespace RemoteJobBoard.Application.Validators;

public class CreateJobPostValidator : AbstractValidator<CreateJobPostDto>
{
    public CreateJobPostValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Job title is required.")
            .MinimumLength(5).WithMessage("Title must be at least 5 characters.")
            .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Job description is required.")
            .MinimumLength(50).WithMessage("Description must be at least 50 characters.");

        RuleFor(x => x.SalaryMin)
            .GreaterThan(0).WithMessage("Minimum salary must be greater than 0.")
            .When(x => x.SalaryMin.HasValue);

        RuleFor(x => x.SalaryMax)
            .GreaterThan(0).WithMessage("Maximum salary must be greater than 0.")
            .GreaterThan(x => x.SalaryMin)
            .WithMessage("Maximum salary must be greater than minimum salary.")
            .When(x => x.SalaryMax.HasValue && x.SalaryMin.HasValue);

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter code like USD, INR, GBP.");

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiry date must be in the future.")
            .When(x => x.ExpiresAt.HasValue);

        RuleFor(x => x.RequiredSkillIds)
            .NotEmpty().WithMessage("At least one required skill must be specified.");
    }
}