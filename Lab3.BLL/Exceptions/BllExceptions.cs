namespace Lab3.BLL.Exceptions;

/// <summary>Базовий виняток бізнес-логіки: рівень представлення ловить саме його.</summary>
public class BllException : Exception
{
    public BllException(string message) : base(message) { }
    public BllException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>Сутність із вказаним ключем не знайдено.</summary>
public class EntityNotFoundException : BllException
{
    public EntityNotFoundException(string key) : base($"Запис із ключем '{key}' не знайдено.") { }
}

/// <summary>Сутність із таким ключем уже існує.</summary>
public class DuplicateEntityException : BllException
{
    public DuplicateEntityException(string key) : base($"Запис із ключем '{key}' уже існує.") { }
}

/// <summary>Дані сутності не відповідають вимогам предметної області.</summary>
public class InvalidEntityDataException : BllException
{
    public InvalidEntityDataException(string message) : base(message) { }
}

/// <summary>Проблема зі сховищем: файл не відкрито, недоступний або має невірний формат.</summary>
public class StorageException : BllException
{
    public StorageException(string message) : base(message) { }
    public StorageException(string message, Exception inner) : base(message, inner) { }
}
