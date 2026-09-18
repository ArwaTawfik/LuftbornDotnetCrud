using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataAccess;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext dbContext;

    public JobApplicationRepository(AppDbContext context)
    {
        this.dbContext = context;
    }

    public async Task<List<JobApplication>> GetAllAsync()
    {
        return await dbContext.JobApplications.ToListAsync();
    }

    public async Task<JobApplication?> GetById(int id)
    {
        return await dbContext.JobApplications.FirstOrDefaultAsync(j => j.Id == id);
    }

    public void Create(JobApplication jobApplication)
    {
        dbContext.JobApplications.Add(jobApplication);
    }

    public void Update(JobApplication jobApplication)
    {
        dbContext.JobApplications.Update(jobApplication);
    }

    public void Delete(JobApplication jobApplication)
    {
        dbContext.JobApplications.Remove(jobApplication);
    }

    public async Task SaveChangeAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}