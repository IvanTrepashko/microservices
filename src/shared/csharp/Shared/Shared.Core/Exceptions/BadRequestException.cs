namespace Shared.Core.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message)
        : base(message) { }

    public BadRequestException(string errorCode, string errorMessage)
        : base($"{errorCode}: {errorMessage}") { }
}
