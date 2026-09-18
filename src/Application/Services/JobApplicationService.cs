using Application.Entities;
using Application.Interfaces;

namespace Application.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository jobApplicationRepository;

    public JobApplicationService(IJobApplicationRepository jobApplicationRepository)
    {
        this.jobApplicationRepository = jobApplicationRepository;
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
