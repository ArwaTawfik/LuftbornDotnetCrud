namespace DataAccess;
using Application.Entities;
public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (context.JobApplications.Any())
            return;

        context.JobApplications.AddRange(
            new JobApplication("Backend Engineer", "Applied via referral at a fintech startup.", DateTime.UtcNow.AddDays(-10), null, ApplicationStatus.Applied),
            new JobApplication(".NET Developer", "Technical screen scheduled for next week.", DateTime.UtcNow.AddDays(-7), null, ApplicationStatus.Interviewing),
            new JobApplication("Full Stack Engineer", "Completed final round, awaiting decision.", DateTime.UtcNow.AddDays(-20), DateTime.UtcNow.AddDays(-2), ApplicationStatus.Offer),
            new JobApplication("Software Engineer", "Position was filled internally.", DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(-15), ApplicationStatus.Rejected),
            new JobApplication("Backend Developer", "Withdrew after accepting another offer.", DateTime.UtcNow.AddDays(-25), DateTime.UtcNow.AddDays(-5), ApplicationStatus.Withdrawn)
        );

        context.SaveChanges();
    }
}