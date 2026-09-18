using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services,
        AuthenticationSettings settings)
    {
        if (!settings.Enabled)
            return services;

        if (string.IsNullOrWhiteSpace(settings.Authority) || string.IsNullOrWhiteSpace(settings.Audience))
            throw new InvalidOperationException(
                $"{AuthenticationSettings.SectionName}:Authority and {AuthenticationSettings.SectionName}:Audience " +
                "must be set when authentication is enabled.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = settings.Authority;
                options.Audience = settings.Audience;
            });
        services.AddAuthorization();

        return services;
    }

    public static WebApplication UseApiAuthentication(this WebApplication app, AuthenticationSettings settings)
    {
        if (!settings.Enabled)
            return app;

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
