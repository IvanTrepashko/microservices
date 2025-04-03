namespace Shared.Core.Exceptions;

public class NotFoundException(string message) : Exception(message);

public class NotFoundException<T>(string id)
    : NotFoundException($"{typeof(T).Name} with id {id} not found");
