using Application.Exceptions;

namespace Application.Validation;

public static class JobApplicationValidator
{
    public static void EnsureEndDateIsNotBeforeStartDate(DateTime applicationStartDate, DateTime? applicationEndDate)
    {
        if (applicationEndDate < applicationStartDate)
            throw new DomainValidationException(nameof(JobApplication.ApplicationEndDate),
                "End date cannot be before the start date.");
    }
}
