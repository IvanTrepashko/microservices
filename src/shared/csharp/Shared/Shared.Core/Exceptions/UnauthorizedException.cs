namespace Shared.Core.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message)
        : base(message) { }

    public UnauthorizedException(string errorCode, string errorMessage)
        : base($"{errorCode}: {errorMessage}") { }
}
