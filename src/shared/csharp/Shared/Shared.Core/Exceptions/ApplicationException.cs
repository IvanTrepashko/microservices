namespace Shared.Core.Exceptions;

public abstract class ApplicationException(string errorCode, string errorMessage)
    : Exception(errorMessage)
{
    public string ErrorCode { get; } = errorCode;

    public string ErrorMessage { get; } = errorMessage;
}
