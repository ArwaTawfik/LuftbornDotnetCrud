using System.ComponentModel.DataAnnotations;
using Application.Entities;

namespace Api.Dtos.Requests;

public record CreateJobApplicationRequest(
    [Required, MaxLength(100)] string Title,
    [Required, MaxLength(2000)] string Description,
    [Required] DateTime ApplicationStartDate,
    DateTime? ApplicationEndDate);

public record UpdateJobApplicationRequest(
    [Required, MaxLength(100)] string Title,
    [Required, MaxLength(2000)] string Description,
    [Required] DateTime ApplicationStartDate,
    DateTime? ApplicationEndDate,
    [Required] ApplicationStatus Status);