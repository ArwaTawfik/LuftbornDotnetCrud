using Api.Dtos.Responses;

namespace Api.Mapping;

public static class JobApplicationMapper
{
    public static JobApplicationResponse ToResponse(JobApplication jobApplication)
    {
        return new JobApplicationResponse
        {
            Id = jobApplication.Id,
            Title = jobApplication.Title,
            Description = jobApplication.Description,
            ApplicationStartDate = jobApplication.ApplicationStartDate,
            ApplicationEndDate = jobApplication.ApplicationEndDate,
            Status = jobApplication.Status
        };
    }
}
