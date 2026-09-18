namespace Api.Authentication;

public class AuthenticationSettings
{
    public const string SectionName = "Authentication";

    public bool Enabled { get; init; }
    public string Authority { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}
