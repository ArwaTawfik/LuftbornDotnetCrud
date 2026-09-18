using System.ComponentModel.DataAnnotations;
using Application.Entities;

public class JobApplication
{
    public int Id { get; set; }
    [Required, MaxLength(100)] public string Title { get; set; }
    [Required, MaxLength(2000)] public string Description { get; set; }
    [Required] public DateTime ApplicationStartDate { get; set; }
    public DateTime? ApplicationEndDate { get; set; }
    [Required] public ApplicationStatus Status { get; set; }

    public JobApplication(string title, string description, DateTime applicationStartDate, DateTime? applicationEndDate,
        ApplicationStatus status)
    {
        this.Title = title;
        this.Description = description;
        this.ApplicationStartDate = applicationStartDate;
        this.ApplicationEndDate = applicationEndDate;
        this.Status = status;
    }
}