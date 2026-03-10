namespace Shared.Core.Exceptions;

public class BadRequestException(string errorCode, string errorMessage)
    : ApplicationException(errorCode, errorMessage);
