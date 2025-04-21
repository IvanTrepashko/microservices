namespace Shared.Core.Exceptions;

public class NotAllowedException : Exception
{
    public NotAllowedException(string message)
        : base(message) { }

    public NotAllowedException(string errorCode, string errorMessage)
        : base($"{errorCode}: {errorMessage}") { }
}
