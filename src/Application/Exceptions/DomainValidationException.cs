namespace Application.Exceptions;

public class DomainValidationException : Exception
{
    public string PropertyName { get; }

    public DomainValidationException(string propertyName, string message) : base(message)
    {
        PropertyName = propertyName;
    }
}
