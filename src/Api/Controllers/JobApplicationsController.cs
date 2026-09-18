using Api.Dtos.Requests;
using Api.Dtos.Responses;
using Api.Mapping;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController : ControllerBase
{
    private readonly ILogger<JobApplicationsController> logger;
    private readonly IJobApplicationService jobApplicationService;

    public JobApplicationsController(ILogger<JobApplicationsController> logger, IJobApplicationService jobApplicationService)
    {
        this.logger = logger;
        this.jobApplicationService = jobApplicationService;
    }

    [HttpGet]
    public async Task<ActionResult<List<JobApplicationResponse>>> GetAll()
    {
        var jobApplications = await jobApplicationService.GetAllAsync();
        return Ok(jobApplications.Select(JobApplicationMapper.ToResponse).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobApplicationResponse>> GetById(int id)
    {
        var jobApplication = await jobApplicationService.GetByIdAsync(id);
        if (jobApplication is null)
            return NotFound();

        return Ok(JobApplicationMapper.ToResponse(jobApplication));
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationResponse>> Create(CreateJobApplicationRequest request)
    {
        var jobApplication = await jobApplicationService.CreateAsync(
            request.Title, request.Description, request.ApplicationStartDate, request.ApplicationEndDate);

        var response = JobApplicationMapper.ToResponse(jobApplication);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateJobApplicationRequest request)
    {
        var updated = await jobApplicationService.UpdateAsync(
            id, request.Title, request.Description, request.ApplicationStartDate, request.ApplicationEndDate, request.Status);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await jobApplicationService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
