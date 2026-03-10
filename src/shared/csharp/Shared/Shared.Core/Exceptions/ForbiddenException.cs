namespace Shared.Core.Exceptions;

public class ForbiddenException(string errorCode, string errorMessage)
    : ApplicationException(errorCode, errorMessage);
