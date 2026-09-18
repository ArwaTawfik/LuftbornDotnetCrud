using System.Text.Json;
using Api.ExceptionHandling;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace Application.Tests.ExceptionHandling;

public class GlobalExceptionHandlerTests
{
    private readonly GlobalExceptionHandler handler = new(NullLogger<GlobalExceptionHandler>.Instance);

    [Fact]
    public async Task Should_ReturnBadRequestWithErrors_When_ValidationExceptionIsThrown()
    {
        var context = CreateHttpContext();
        var exception = new ValidationException(new[]
        {
            new ValidationFailure("ApplicationEndDate", "End date cannot be before the start date.")
        });

        var handled = await handler.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Equal("application/problem+json", context.Response.ContentType);

        using var body = ReadBody(context);
        var message = body.RootElement.GetProperty("errors").GetProperty("ApplicationEndDate")[0].GetString();
        Assert.Equal("End date cannot be before the start date.", message);
    }

    [Fact]
    public async Task Should_GroupMessagesByProperty_When_ValidationExceptionHasSeveralErrors()
    {
        var context = CreateHttpContext();
        var exception = new ValidationException(new[]
        {
            new ValidationFailure("Title", "Title is required."),
            new ValidationFailure("Title", "Title is too long."),
            new ValidationFailure("Description", "Description is required.")
        });

        await handler.TryHandleAsync(context, exception, CancellationToken.None);

        using var body = ReadBody(context);
        var errors = body.RootElement.GetProperty("errors");
        Assert.Equal(2, errors.GetProperty("Title").GetArrayLength());
        Assert.Equal(1, errors.GetProperty("Description").GetArrayLength());
    }

    [Fact]
    public async Task Should_ReturnGenericServerErrorWithoutDetails_When_UnexpectedExceptionIsThrown()
    {
        var context = CreateHttpContext();
        var exception = new InvalidOperationException("connection string leaked: Server=prod;Password=secret");

        var handled = await handler.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);

        context.Response.Body.Position = 0;
        var rawBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Assert.DoesNotContain("secret", rawBody);
        Assert.DoesNotContain("connection string", rawBody);
        Assert.Contains("An unexpected error occurred.", rawBody);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static JsonDocument ReadBody(DefaultHttpContext context)
    {
        context.Response.Body.Position = 0;
        return JsonDocument.Parse(context.Response.Body);
    }
}
