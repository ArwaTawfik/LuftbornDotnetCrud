using Application.Entities;
using Application.Interfaces;
using FluentValidation;

namespace Application.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository jobApplicationRepository;
    private readonly IValidator<JobApplication> jobApplicationValidator;

    public JobApplicationService(IJobApplicationRepository jobApplicationRepository,
        IValidator<JobApplication> jobApplicationValidator)
    {
        this.jobApplicationRepository = jobApplicationRepository;
        this.jobApplicationValidator = jobApplicationValidator;
    }

    public Task<List<JobApplication>> GetAllAsync()
    {
        return jobApplicationRepository.GetAllAsync();
    }

    public Task<JobApplication?> GetByIdAsync(int id)
    {
        return jobApplicationRepository.GetById(id);
    }

    public async Task<JobApplication> CreateAsync(string title, string description, DateTime applicationStartDate,
        DateTime? applicationEndDate)
    {
        var jobApplication = new JobApplication(title, description, applicationStartDate, applicationEndDate,
            ApplicationStatus.Applied);
        await jobApplicationValidator.ValidateAndThrowAsync(jobApplication);

        jobApplicationRepository.Create(jobApplication);
        await jobApplicationRepository.SaveChangeAsync();
        return jobApplication;
    }

    public async Task<bool> UpdateAsync(int id, string title, string description, DateTime applicationStartDate,
        DateTime? applicationEndDate, ApplicationStatus status)
    {
        var jobApplication = await jobApplicationRepository.GetById(id);

        if (jobApplication == null)
            return false;
        jobApplication.Title = title;
        jobApplication.Description = description;
        jobApplication.ApplicationStartDate = applicationStartDate;
        jobApplication.ApplicationEndDate = applicationEndDate;
        jobApplication.Status = status;
        await jobApplicationValidator.ValidateAndThrowAsync(jobApplication);

        jobApplicationRepository.Update(jobApplication);
        await jobApplicationRepository.SaveChangeAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var jobApplication = await jobApplicationRepository.GetById(id);

        if (jobApplication == null)
            return false;
        jobApplicationRepository.Delete(jobApplication);
        await jobApplicationRepository.SaveChangeAsync();
        return true;
    }
}
