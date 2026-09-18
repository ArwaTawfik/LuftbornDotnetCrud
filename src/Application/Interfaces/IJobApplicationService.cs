using Application.Entities;

namespace Application.Interfaces;

public interface IJobApplicationService
{
    Task<List<JobApplication>> GetAllAsync();
    Task<JobApplication?> GetByIdAsync(int id);

    Task<JobApplication> CreateAsync(string title, string description, DateTime applicationStartDate,
        DateTime? applicationEndDate);

    Task<bool> UpdateAsync(int id, string title, string description, DateTime applicationStartDate,
        DateTime? applicationEndDate, ApplicationStatus status);

    Task<bool> DeleteAsync(int id);
}
