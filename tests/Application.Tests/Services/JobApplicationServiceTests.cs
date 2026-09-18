using Application.Entities;
using Application.Interfaces;
using Application.Services;
using Application.Validation;
using FluentValidation;
using Moq;

namespace Application.Tests.Services;

public class JobApplicationServiceTests
{
    private static readonly DateTime Start = new(2026, 9, 18);

    private readonly Mock<IJobApplicationRepository> repository = new();
    private readonly JobApplicationService service;

    public JobApplicationServiceTests()
    {
        service = new JobApplicationService(repository.Object, new JobApplicationValidator());
    }

    [Fact]
    public async Task Should_SetStatusToApplied_When_CreatingApplication()
    {
        var created = await service.CreateAsync("Backend Engineer", "Description", Start, null);

        Assert.Equal(ApplicationStatus.Applied, created.Status);
    }

    [Fact]
    public async Task Should_CreateAndSave_When_CreatingValidApplication()
    {
        await service.CreateAsync("Backend Engineer", "Description", Start, Start.AddDays(3));

        repository.Verify(r => r.Create(It.Is<JobApplication>(a =>
            a.Title == "Backend Engineer" && a.Status == ApplicationStatus.Applied)), Times.Once);
        repository.Verify(r => r.SaveChangeAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_ThrowAndNotSave_When_CreatingWithEndDateBeforeStartDate()
    {
        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateAsync("Backend Engineer", "Description", Start, Start.AddDays(-1)));

        repository.Verify(r => r.Create(It.IsAny<JobApplication>()), Times.Never);
        repository.Verify(r => r.SaveChangeAsync(), Times.Never);
    }

    [Fact]
    public async Task Should_ReturnFalse_When_UpdatingApplicationThatDoesNotExist()
    {
        repository.Setup(r => r.GetById(999)).ReturnsAsync((JobApplication?)null);

        var updated = await service.UpdateAsync(999, "Title", "Description", Start, null, ApplicationStatus.Offer);

        Assert.False(updated);
        repository.Verify(r => r.Update(It.IsAny<JobApplication>()), Times.Never);
        repository.Verify(r => r.SaveChangeAsync(), Times.Never);
    }

    [Fact]
    public async Task Should_ApplyChangesAndSave_When_UpdatingExistingApplication()
    {
        var existing = ExistingApplication();
        repository.Setup(r => r.GetById(existing.Id)).ReturnsAsync(existing);

        var updated = await service.UpdateAsync(existing.Id, "New title", "New description", Start.AddDays(1),
            Start.AddDays(5), ApplicationStatus.Interviewing);

        Assert.True(updated);
        Assert.Equal("New title", existing.Title);
        Assert.Equal("New description", existing.Description);
        Assert.Equal(Start.AddDays(1), existing.ApplicationStartDate);
        Assert.Equal(Start.AddDays(5), existing.ApplicationEndDate);
        Assert.Equal(ApplicationStatus.Interviewing, existing.Status);
        repository.Verify(r => r.Update(existing), Times.Once);
        repository.Verify(r => r.SaveChangeAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_ThrowAndNotSave_When_UpdatingWithEndDateBeforeStartDate()
    {
        var existing = ExistingApplication();
        repository.Setup(r => r.GetById(existing.Id)).ReturnsAsync(existing);

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.UpdateAsync(existing.Id, "Title", "Description", Start, Start.AddDays(-1),
                ApplicationStatus.Applied));

        repository.Verify(r => r.Update(It.IsAny<JobApplication>()), Times.Never);
        repository.Verify(r => r.SaveChangeAsync(), Times.Never);
    }

    [Fact]
    public async Task Should_ReturnFalse_When_DeletingApplicationThatDoesNotExist()
    {
        repository.Setup(r => r.GetById(999)).ReturnsAsync((JobApplication?)null);

        var deleted = await service.DeleteAsync(999);

        Assert.False(deleted);
        repository.Verify(r => r.Delete(It.IsAny<JobApplication>()), Times.Never);
        repository.Verify(r => r.SaveChangeAsync(), Times.Never);
    }

    [Fact]
    public async Task Should_RemoveAndSave_When_DeletingExistingApplication()
    {
        var existing = ExistingApplication();
        repository.Setup(r => r.GetById(existing.Id)).ReturnsAsync(existing);

        var deleted = await service.DeleteAsync(existing.Id);

        Assert.True(deleted);
        repository.Verify(r => r.Delete(existing), Times.Once);
        repository.Verify(r => r.SaveChangeAsync(), Times.Once);
    }

    private static JobApplication ExistingApplication()
    {
        return new JobApplication("Old title", "Old description", Start, null, ApplicationStatus.Applied) { Id = 5 };
    }
}
