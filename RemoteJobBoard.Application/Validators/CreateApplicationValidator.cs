
using FluentValidation;
using RemoteJobBoard.Application.DTOs.Application;

namespace RemoteJobBoard.Application.Validators;

public class CreateApplicationValidator : AbstractValidator<CreateApplicationDto>
{
    public CreateApplicationValidator()
    {
        RuleFor(x => x.JobPostId)
            .NotEmpty().WithMessage("Job post is required.");

        RuleFor(x => x.CoverLetter)
            .MaximumLength(2000).WithMessage("Cover letter cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.CoverLetter));
    }
}