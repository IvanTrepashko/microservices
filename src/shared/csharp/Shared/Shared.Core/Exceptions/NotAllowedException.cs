namespace Shared.Core.Exceptions;

public class NotAllowedException(string errorCode, string errorMessage)
    : ApplicationException(errorCode, errorMessage);
