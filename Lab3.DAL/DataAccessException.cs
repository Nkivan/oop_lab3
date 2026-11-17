namespace Lab3.DAL;

/// <summary>Помилка рівня доступу до даних (файл недоступний, невірний формат тощо).</summary>
public class DataAccessException : Exception
{
    public DataAccessException(string message) : base(message) { }
    public DataAccessException(string message, Exception inner) : base(message, inner) { }
}
