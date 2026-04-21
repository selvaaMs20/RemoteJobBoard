
using FluentValidation;
using RemoteJobBoard.Application.DTOs.JobseekerProfile;

namespace RemoteJobBoard.Application.Validators;

public class UpdateJobSeekerProfileValidator : AbstractValidator<UpdateJobSeekerProfileDto>
{
    public UpdateJobSeekerProfileValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Bio)
            .MaximumLength(1000).WithMessage("Bio cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Bio));

        RuleFor(x => x.YearsOfExperience)
            .InclusiveBetween(0, 50).WithMessage("Years of experience must be between 0 and 50.");

        RuleFor(x => x.LinkedInUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("LinkedIn URL must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.LinkedInUrl));

        RuleFor(x => x.GitHubUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("GitHub URL must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.GitHubUrl));

        RuleFor(x => x.SkillIds)
            .NotNull().WithMessage("Skills list cannot be null.");
    }
}