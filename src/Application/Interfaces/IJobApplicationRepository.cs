namespace Application.Interfaces;

public interface IJobApplicationRepository
{
    Task<List<JobApplication>> GetAllAsync();
    Task<JobApplication?> GetById(int id);
    void Create(JobApplication jobApplication);
    void Update(JobApplication jobApplication);
    void Delete(JobApplication jobApplication);
    Task SaveChangeAsync();

}