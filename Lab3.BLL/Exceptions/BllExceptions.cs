namespace Lab3.BLL.Exceptions;


public class BllException : Exception
{
    public BllException(string message) : base(message) { }
    public BllException(string message, Exception inner) : base(message, inner) { }
}


public class EntityNotFoundException : BllException
{
    public EntityNotFoundException(string key) : base($"Запис із ключем '{key}' не знайдено.") { }
}


public class DuplicateEntityException : BllException
{
    public DuplicateEntityException(string key) : base($"Запис із ключем '{key}' уже існує.") { }
}


public class InvalidEntityDataException : BllException
{
    public InvalidEntityDataException(string message) : base(message) { }
}


public class StorageException : BllException
{
    public StorageException(string message) : base(message) { }
    public StorageException(string message, Exception inner) : base(message, inner) { }
}
