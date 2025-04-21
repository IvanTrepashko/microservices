namespace Shared.Core.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message)
        : base(message) { }

    public ForbiddenException(string errorCode, string errorMessage)
        : base($"{errorCode}: {errorMessage}") { }
}
