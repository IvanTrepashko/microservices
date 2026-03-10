namespace Shared.Core.Exceptions;

public class UnauthorizedException(string errorCode, string errorMessage)
    : ApplicationException(errorCode, errorMessage);
