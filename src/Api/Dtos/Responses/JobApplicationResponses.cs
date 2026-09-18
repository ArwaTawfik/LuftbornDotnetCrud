using Application.Entities;

namespace Api.Dtos.Responses;

public class JobApplicationResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ApplicationStartDate { get; set; }
    public DateTime? ApplicationEndDate { get; set; }
    public ApplicationStatus Status { get; set; }
}