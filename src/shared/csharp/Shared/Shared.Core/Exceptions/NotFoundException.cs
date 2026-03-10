namespace Shared.Core.Exceptions;

public class NotFoundException(string errorCode, string errorMessage)
    : ApplicationException(errorCode, errorMessage);

public class NotFoundException<T>(string id)
    : NotFoundException("NOT_FOUND", $"{typeof(T).Name} with id {id} not found");
