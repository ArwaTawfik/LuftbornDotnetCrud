using System.Text.Json.Serialization;
using Api.Authentication;
using Api.ExceptionHandling;
using Application.Interfaces;
using Application.Services;
using Application.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);

var authenticationSettings = builder.Configuration
    .GetSection(AuthenticationSettings.SectionName)
    .Get<AuthenticationSettings>() ?? new AuthenticationSettings();

builder.Services.AddApiAuthentication(authenticationSettings);
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IValidator<JobApplication>, JobApplicationValidator>();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("AllowAngularDev");
app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseApiAuthentication(authenticationSettings);

var controllers = app.MapControllers();
if (authenticationSettings.Enabled)
    controllers.RequireAuthorization();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    DbInitializer.Seed(context);
}

app.Run();