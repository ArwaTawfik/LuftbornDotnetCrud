using FluentValidation;

namespace Application.Validation;

public class JobApplicationValidator : AbstractValidator<JobApplication>
{
    public JobApplicationValidator()
    {
        RuleFor(application => application.ApplicationEndDate)
            .GreaterThanOrEqualTo(application => application.ApplicationStartDate)
            .WithMessage("End date cannot be before the start date.");
    }
}
