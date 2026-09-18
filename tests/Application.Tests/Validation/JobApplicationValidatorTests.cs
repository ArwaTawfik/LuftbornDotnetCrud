using Application.Entities;
using Application.Validation;
using FluentValidation.TestHelper;

namespace Application.Tests.Validation;

public class JobApplicationValidatorTests
{
    private const string EndBeforeStartMessage = "End date cannot be before the start date.";

    private readonly JobApplicationValidator validator = new();

    [Fact]
    public void Should_HaveError_When_EndDateIsBeforeStartDate()
    {
        var application = CreateApplication(new DateTime(2026, 9, 18), new DateTime(2026, 9, 17));

        var result = validator.TestValidate(application);

        result.ShouldHaveValidationErrorFor(a => a.ApplicationEndDate).WithErrorMessage(EndBeforeStartMessage);
    }

    [Fact]
    public void Should_NotHaveError_When_EndDateIsAfterStartDate()
    {
        var application = CreateApplication(new DateTime(2026, 9, 18), new DateTime(2026, 9, 19));

        var result = validator.TestValidate(application);

        result.ShouldNotHaveValidationErrorFor(a => a.ApplicationEndDate);
    }

    [Fact]
    public void Should_NotHaveError_When_EndDateIsTheSameDayAsStartDate()
    {
        var application = CreateApplication(new DateTime(2026, 9, 18), new DateTime(2026, 9, 18));

        var result = validator.TestValidate(application);

        result.ShouldNotHaveValidationErrorFor(a => a.ApplicationEndDate);
    }

    [Fact]
    public void Should_NotHaveError_When_EndDateIsNotSet()
    {
        var application = CreateApplication(new DateTime(2026, 9, 18), null);

        var result = validator.TestValidate(application);

        result.ShouldNotHaveValidationErrorFor(a => a.ApplicationEndDate);
    }

    private static JobApplication CreateApplication(DateTime start, DateTime? end)
    {
        return new JobApplication("Backend Engineer", "Test description", start, end, ApplicationStatus.Applied);
    }
}
